using NAudio.Wave;
using NAudio.Wave.SampleProviders;

namespace T4Dungeon.Game.Utils;

/// <summary>
/// Centralized audio system for the game.
/// Handles background music looping and one-shot sound effect playback.
/// All audio files should be placed in the Resources/Audio folder
/// relative to the executable.
/// </summary>
public static class AudioManager
{
    // ─── State ────────────────────────────────────────────────────
    private static WaveOutEvent? _musicOutput;
    private static AudioFileReader? _musicReader;
    private static WaveOutEvent? _sfxOutput;
    private static float _musicVolume = 0.5f;
    private static float _sfxVolume = 0.8f;
    private static bool _enabled = true;

    private static readonly string AudioRoot =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Audio");

    // ─── Public Controls ──────────────────────────────────────────

    /// <summary>Gets or sets whether audio is enabled globally.</summary>
    public static bool Enabled
    {
        get => _enabled;
        set
        {
            _enabled = value;
            if (!_enabled) StopMusic();
        }
    }

    /// <summary>Music volume from 0.0 to 1.0.</summary>
    public static float MusicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = Math.Clamp(value, 0f, 1f);
            if (_musicOutput != null) _musicOutput.Volume = _musicVolume;
        }
    }

    /// <summary>Sound effect volume from 0.0 to 1.0.</summary>
    public static float SfxVolume
    {
        get => _sfxVolume;
        set => _sfxVolume = Math.Clamp(value, 0f, 1f);
    }

    // ─── Music ────────────────────────────────────────────────────

    /// <summary>
    /// Plays a music track on loop, replacing any currently playing music.
    /// Fades out the previous track before starting the new one.
    /// </summary>
    /// <param name="track">The music track to play (from <see cref="Music"/>).</param>
    public static void PlayMusic(string track)
    {
        if (!_enabled) return;

        string path = Path.Combine(AudioRoot, "Music", track);
        if (!File.Exists(path)) return;

        StopMusic();

        try
        {
            _musicReader = new AudioFileReader(path) { Volume = _musicVolume };
            var looping = new LoopStream(_musicReader);
            _musicOutput = new WaveOutEvent();
            _musicOutput.Init(looping);
            _musicOutput.Play();
        }
        catch { /* Audio failure should never crash the game */ }
    }

    /// <summary>Stops the currently playing music track.</summary>
    public static void StopMusic()
    {
        _musicOutput?.Stop();
        _musicOutput?.Dispose();
        _musicReader?.Dispose();
        _musicOutput = null;
        _musicReader = null;
    }

    // ─── Sound Effects ────────────────────────────────────────────

    /// <summary>
    /// Plays a one-shot sound effect. Does not interrupt music.
    /// Multiple SFX calls are fire-and-forget — they don't block.
    /// </summary>
    /// <param name="sfx">The sound effect to play (from <see cref="Sfx"/>).</param>
    public static void PlaySfx(string sfx)
    {
        if (!_enabled) return;

        string path = Path.Combine(AudioRoot, "Sfx", sfx);
        if (!File.Exists(path)) return;

        // Fire and forget on a thread pool thread so it never blocks the game loop
        Task.Run(() =>
        {
            try
            {
                using var reader = new AudioFileReader(path) { Volume = _sfxVolume };
                using var output = new WaveOutEvent();
                output.Init(reader);
                output.Play();
                while (output.PlaybackState == PlaybackState.Playing)
                    Thread.Sleep(10);
            }
            catch { }
        });
    }

    /// <summary>Releases all audio resources. Call on game exit.</summary>
    public static void Dispose()
    {
        StopMusic();
        _sfxOutput?.Dispose();
    }
}

/// <summary>
/// Wraps an audio stream to loop it indefinitely.
/// Used internally by AudioManager for background music.
/// </summary>
internal class LoopStream : WaveStream
{
    private readonly WaveStream _source;

    public LoopStream(WaveStream source) => _source = source;

    public override WaveFormat WaveFormat => _source.WaveFormat;
    public override long Length => _source.Length;
    public override long Position
    {
        get => _source.Position;
        set => _source.Position = value;
    }

    public override int Read(byte[] buffer, int offset, int count)
    {
        int totalRead = 0;
        while (totalRead < count)
        {
            int read = _source.Read(buffer, offset + totalRead, count - totalRead);
            if (read == 0)
                _source.Position = 0; // Loop back to start
            else
                totalRead += read;
        }
        return totalRead;
    }
}