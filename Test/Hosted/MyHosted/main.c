#include <stdio.h>

extern int GetVersion();

int main()
{
    printf("Hello world3\n");
    printf("Version: %d\n", GetVersion());
}