# UnixBuilder


## Examples
Build with Host toolchain
```
../latest.sh build -t Hosted/MyHosted
```

Build with specified toolchain
```
../latest.sh build -t Hosted/MyHosted -c Artyz7
```

Build project
```
../latest.sh build -p Full
```

Build project
```
../latest.sh build -p Hosted -c Host
```

Package project (default is zip)
```
../latest.sh package -p Hosted
```