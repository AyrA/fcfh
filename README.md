# fcfh
4chan file host

This application encodes information into images so they can be uploaded to 4chan or any other image hosting site.

## Modes of operation

### Encoding

Depending on the image format you might be able to encode information in multiple ways.
Encoding will always also store the original file name for later decoding.
In the case of pixel mode,
the application tries to generate images that are as close to a square as possible.

#### PNG

PNG can be used in two modes, pixel mode and header mode.
Header mode inserts a custom PNG header with your data in it,
this can be done multiple times to encode multiple files into a single image.
For header mode you need an existing PNG image.
Ideally this image contains instructions on how to decode it.
Pixel mode will use the image data to save your content.
Because PNG is a compressed image format you will not be able to see
the original binary content in a hex editor.
You need to convert the image to a BMP first or read the pixels individually.

#### BMP

Bitmap files are weird because the image data starts at the bottom left and goes up.
You can store the information as pixel data only.
You have the option to store it in a way that the raw binary data is still in order.

### Decode

Decoding detects the type of encoding automatically.
You only need to know if it was encrypted or not.

### Encryption

You can store content encrypted.
This uses industry standard AES encryption.
The key generation was made artificially more difficult to reduce the risks of brute-force attacks.

**DO NOT FORGET THE PASSWORD!**

There is no known backdoor or weakness in AES that would help you recover the content if you no longer have the password.

## Stability of images

You can only decode your information if the image was not altered.

### Pixel mode

The pixels must not be changed as every pixel is needed for decoding with very few exceptions.
This means you can only use lossless formats like bmp or png and are free to convert between them.
If you convert to a lossy format like JPEG, the information is lost.

### Header mode

Headers usually don't get converted to the new format if changing image formats
but they sometimes don't get erased when uploading the image to a 3rd party,
this makes them harder to detect, unless you store an image that looks way too small for its resolution.

# Usage Example

I made a simple [JavaScript example](https://cable.ayra.ch/imgplay/).
This Example implementation will only decode the first embedded data block found in PNG headers.
It does not supports pixel mode and will not check if the data is actually a supported audio format.

# TODO

Not necessarily in order

- [X] Implement Pixel mode Encode
- [X] Implement Pixel mode Decode
- [X] Implement Header mode Encode
- [X] Implement Header mode Decode
- [X] Make integers consistant (network byte order)
- [X] File name functions for Decode
- [X] Implement Encryption component
- [X] Implement Decryption component
- [X] Implement Command line switches
- [ ] GUI
- [ ] Publish (some sort of) standard
