using System.Text;

public class TextMap
{
    private readonly byte[][] cells = [];
    private int map_size = 0;

    public TextMap(string FileName)
    {
        string[] map_lines = File.ReadAllLines(FileName);
        map_size = map_lines.Length; // Assume square

        cells = new byte[map_size][];
        int y = 0;

        foreach (string line in map_lines)
        {
            byte[] bytes = Encoding.ASCII.GetBytes(line);
            cells[y++] = bytes;
        }
    }

    /** Get a full screen's worth of cell data, with out of range replaced by checker for screen edges*/
    public byte[] GetScreen(uint x, uint y, uint width, uint height)
    {
        byte[] screen = new byte[width * height];
        int index = 0;

        for (uint yy = 0; yy < height; yy++)
        {
            for (uint xx = 0; xx < width; xx++)
            {
                uint off_x = x + xx;
                uint off_y = y + yy;
                try
                {
                    if ((off_x < map_size) && (off_y < map_size))
                    {
                        screen[index] = cells[off_x][off_y];
                    }
                    else
                    {
                        screen[index] = 230;  // Checker
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    screen[index] = 255;  // Big Checker

                }
                index++;
            }
        }

        return screen;
    }
}
