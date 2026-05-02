using System;

public class InputSystem
{
    public int GetSelection(int maxOption)
    {
        while (true)
        {
            var key = Console.ReadKey(true);

            if (char.IsDigit(key.KeyChar))
            {
                int value = key.KeyChar - '1';

                if (value >= 0 && value < maxOption)
                    return value;
            }
        }
    }
}