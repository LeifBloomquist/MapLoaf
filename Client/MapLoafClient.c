/* MapLoafClient.c */

#include <cbm.h>
#include <peekpoke.h>
#include <conio.h>
#include <stdio.h>
#include <stdlib.h>
#include <string.h>
#include <unistd.h>
#include <time.h>

#ifdef LOCAL
#define BASE_URL "http://192.168.7.99/test/"
#else
#define BASE_URL "http://vortex.jammingsignal.com:8065/ml/maps/"
#endif

char url[100];

void error(char* message)
{
    clrscr();
    cprintf("%s\n", message);
}

int load_text_screen(char* filename)
{
    return cbm_load(filename, 9, NULL);
}

int refresh(int x, int y)
{
    int result;
    bordercolor(11);
    sprintf(url, BASE_URL"map.prg?x=%d&y=%d", x, y);
    result = load_text_screen(url);
    bordercolor(0);
    return result;
}

void main() 
{
    char c = 0;
    int result = 0;

    unsigned int x = 0;
    unsigned int y = 0;    

    clrscr();
    bordercolor(0);
    refresh(x, y);

    while (1)
    {
        c = cgetc();

        switch (c)
        {
            case 19:  // Home
                x = 0;
                y = 0;
                break;

            case 145:  // Cursor up
                if (y > 0) y--;
                break;

            case 17:  // Cursor down
                if (y <= 1000) y++;
                break;

            case 157:  // Cursor left
                if (x > 0) x--;
                break;

            case 29:  // Cursor right
                if (y <= 1000) x++;
                break;

            default:
                continue;
        }

        refresh(x, y);
    }
}
