# TestCore Package

A Unity package providing core test functionality.

## Installation

### Via manifest.json

Add the following line to your `Packages/manifest.json`:

```json
"com.jarzykk.test-core": "https://github.com/Jarzykk/TestCore-Package.git"
```

### Local Development

For local development, you can add the package via file path:

```json
"com.jarzykk.test-core": "file:../../TestCore-Package"
```

## Package Structure

- `Runtime/` - Runtime code and assembly definitions
  - `TestService.cs` - Main service class
  - `TestCore.asmdef` - Assembly definition

## License

See LICENSE.md for details.