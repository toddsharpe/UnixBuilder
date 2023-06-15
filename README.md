# UnixBuilder
Very simple build system for small projects, accepting various unix style toolchains (for cross-compilation, etc).

## Usage
* Build target: ```../latest.sh <[build, package, run]> -t <target> -c <toolchain,"Host">```
* Build package: ```../latest.sh <[build, package, run]> -p <package> -c <toolchain, "Host">```

## Building
```
cd Src
dotnet build
```

# Build System Setup
Create sources, toolchains, then targets. Optionally create projects (collection of targets, can be packaged).

## Config Files

### Sources
Create ```SOURCES``` file at root of sources directory (paths will be relative to this directory). Example: [SOURCES](Test/SOURCES). Defines build output path (```OutDir```), directories and toolchains, each entry in "Toolchains" must have a toolchain file. Optionally can define Config root (```ConfigDir```) to be referenced by project files.

### Toolchains
Defines each unix toolchain that can be invoked to compile sources. Examples: [TOOLCHAIN_Host](Test/TOOLCHAIN_Host), [TOOLCHAIN_Arty](Test/TOOLCHAIN_Arty). Defines binary directory, toolchain prefix, and flags to use.

### Targets
Defines binary name, C/C++ files, and flags. Source files are relative to current directory or can be relative to sources directory be prefixing with ">". Example: [Test/Common/MyApp/TARGET](Test/Common/MyApp/TARGET).

### Projects
Defines list of targets and optionally configs. Example: [Test/PROJECT_Hosted](Test/PROJECT_Hosted). These can be packaged into a zip file with the heierarchy:
```
bin/<compiled binaries>
dat/<list of config files>
```

Entries in ```[Configs]``` section can be directory or files. These can also use ">" syntax to be relative to ```ConfigDir```.

## Example Usage
Build one target with default(Host) toolchain
```
../latest.sh build -t Hosted/MyHosted
```

Build and run one target with default(Host) toolchain
```
../latest.sh run -t Hosted/MyHosted
```

Build one target specified toolchain (cross-compile)
```
../latest.sh build -t Hosted/MyHosted -c Artyz7
```

Build project
```
../latest.sh build -p Full
```

Build project with toolchain
```
../latest.sh build -p Hosted -c Host
../latest.sh build -p Full -c Host
```

Package project
```
../latest.sh package -p Hosted
```