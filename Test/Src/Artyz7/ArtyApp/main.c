#include <stdio.h>

extern int GetVersion();

int main()
{
    printf("Hello world\n");
    printf("Version: %d\n", GetVersion());
}