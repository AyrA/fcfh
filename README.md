# fcfh

4chan file host

This application encodes information into images so they can be uploaded to 4chan or most other image hosting sites.

## Usage

This application can be used with a GUI or a console.
It detects the appropriate mode automatically.

### Command line arguments

*You can also use the argument `/?` to bring up the following help*

	fcfh.exe /{e|d} [/s] <infile> [outfile] [/readable] [/header source] [/p|/pass password] [/fn name]

	Encodes and decodes information to/from images.
	This tool is not using steganography and literally just stores the data in an
	image container, but ensures that said container is valid.

	/e          - Encode a file into an image
	/d          - Decode a file from an image
	/s          - Treat input argument as string rather than file.
				  Only valid when encoding.
	infile      - Source file
	outfile     - Destination file
				  If not specified it assumes png for header encoded data,
				  Assumes PNG for pixel data without '/readable'
				  Assumes BMP for pixel data with '/readable'.
	/readable   - Encodes content in binary readable form.
				  Only has an effect if encoding to 24 bit bitmap (.bmp),
				  because bitmap files are stored bottom up.
	/header     - Store content in a header instead of pixel data.
				  Be aware that some applications strip unknown headers.
				  The source argument is the file that is used as template.
				  This only works on PNG files.
				  Note: Some editors will strip unknown headers if you edit
				  the image file.
	/pass       - Encrypt/Decrypt using the given password.
				  This uses proper AES, recovery of content is impossible
				  if the password is lost.
	/p            Same as /pass but prompts for a password at runtime.
	/fn         - Use the given file name for the header instead of the
				  supplied name. If /s is specified and /fn is not,
				  it will default to text.txt. This argument is for
				  encoding only. Decoding uses the 'outfile' argument

	Note: When decoding, the arguments /readable and /header are auto-detected

## Supported image formats

Fcfh supports bitmap files and png files.
PNG is generally recommended.
JPEG is not supported because it destroys the pixel data due to lossy compression.
Header mode is not available for BMP files.

## Data integrity

Regardless of the header or pixel mode, data can get somewhat easily corrupted.
If you do decide to publish fcfh images,
download them yourself and try to decode.

Fcfh does not store checksums of data except for when you encrypt them.
This will not prevent data corruption, but will identify it when decoding.

## Modes of operation

This application can encode as well as decode files inside of images.

### Encoding pixel data

Pixel data is a bit more complicated to encode than header data,
but it works with all image types that don't use lossless compression.

Pros:

- Supported by all formats
- Creates a visual representation of the file
- Works without requiring a pre-existing image

Cons:

- Only one file can be encoded in an image this way
- Easy to destroy via image conversion to JPEG and some other optimization methods
- The fact that a file was encoded in the image, is usually obvious

Pixel data is quite simple to encode.
An image size is chosen that is large enough to contain the entire file.
Fcfh will select size parameters in a way that creates square images.

The data is stored in the red, green and blue color channels.
Fcfh uses 24 bit colors, resulting in 3 bytes for each pixel.
This color mode is very common for images and supported by all applications.

Image formats that compress pixel data (such as PNG) make the data unreadable in a hex editor without decompression.
If it's important that the data can be read as-is in a hex editor, use header mode.

#### Pixel data in BMP files

BMP files are a bit special in how pixel data is stored.
It has two very unconventional behaviors:

- Pixel data is aligned to a 4-byte boundary on every line
- Lines are stored from the bottom up instead of top down

Fcfh will handle both problems.
The computed line width is adjusted to guarantee it's a multiple of 4,
The source data can optionally be read linewise in reverse
so it's normally readable in a hex editor.

When decoding, fcfh can deal with files that are not 4-byte aligned.

### Encoding header data

Headers are simpler to encode than pixel data and come with some advantages and drawbacks.

Pros:

- The encoded data is invisible
- Harder to accidentally destroy in image editors
- Not bound by any pixel data restrictions
- Multiple files can be encoded into a single image
- Pixel data stays available for free use, for example for instructions.

Cons:

- Requires an existing PNG image
- Many websites will strip headers from PNG images if the file is too large
- Removal/Corruption of the header is invisible to people
- Is PNG specific and will usually not survive conversion to JPEG

Header mode is invisible to humans beyond size discrepancies.
If a large amount of data is stored,
it does become noticeable if the visible image data itself is very small.
If you encode a large file into a header,
make sure the image also has a lot of pixel data to hide the fact.

Some websites (notably 4chan) will strip headers if the image size is too large for the pixel data.
Storing simple strings like URLs or secret messages usually works regardless of image size.

### Decoding

Fcfh can decode in header mode as well as pixel mode.

#### Console

Decoding detects the type of encoding automatically (prefers header mode over pixel mode).
You only need to know if it was encrypted or not.

#### GUI

The GUI can decode header and pixel mode at the same time.
It will detect encryption automatically and prompt for a password if necessary.

Note: The password is not stored. You will be prompted individually for each encrypted file.

## Encryption

You can store content encrypted.
This uses industry standard AES encryption.
The key generation was made artificially more difficult to reduce the risks of brute-force attacks.
Encrypting the content also adds an integrity check and refuses decryption if the check fails.

**DO NOT FORGET THE PASSWORD!**

There is no known backdoor or weakness in AES that would help you recover the content if you no longer have the password.

## Stability of images

Data can get corrupted or deleted under some circumstances.
When this happens depends on how the data was encoded and where the image was uploaded.

### Pixel mode

The pixels must not be changed as every pixel is needed for decoding.
This means you can only use lossless formats like bmp or png and are free to convert between them.
If you convert to a lossy format like JPEG, the information is lost.

### Header mode

Headers usually don't get converted to the new format if changing image formats
but they sometimes don't get erased when uploading the image to a 3rd party,
this makes them harder to detect, unless you create an image that looks way too small for its resolution.

# Browser Usage

I made a simple [JavaScript example](https://cable.ayra.ch/imgplay/).
This Example implementation will only decode the first embedded data block found in PNG headers.
It does not supports pixel mode.

The example assumes the data is audio, but will not check if it's actually a supported format.
An example image is provided.

# Standards

this chapter describes how the data is encoded.

All Integers are stored in network byte order.
According to [RFC 1700](https://tools.ietf.org/html/rfc1700) this is big endian.

| Field      | Type  | Length (bytes) | Description         |
| ---------- | ----- | -------------- | ------------------- |
| ID         | Bytes | 6              | Always "BMPENC"     |
| NameLength | Int32 | 4              | Length of File Name |
| Name       | UTF-8 | NameLength     | File name           |
| DataLength | Int32 | 4              | Length of Data      |
| Data       | Bytes | DataLength     | Payload             |

## Signed vs. Unsigned

Because unsigned integers are more difficult to deal with than signed integers in some languages,
all integers are to be treated as signed.

Encoding:

Invalid integer sequences (such as a negative length of a field) are not permitted.

Decoding:

Invalid integer sequences (such as a negative length of a field) should cause the decoder to abort.

## Detailed Field description

### ID

The ID field **MUST** be made up of the byte sequence `42 4D 50 45 4E 43`, which translates to ASCII `BMPENC` but is to be treated as a sequence of octets and not characters.

### NameLength

This is the length of the file name.
The length is specified in raw bytes and not the number of UTF-8 characters.

### Name

this is the file name as UTF-8.
The file name **MUST NOT** contain a byte order mark.
The file name **MUST NOT** be null terminated.
An encoder **MUST NOT** store path information, only the file name.
A decoder **MUST NOT** interpret the path name.

### DataLength

This is the length of the payload in bytes.
This field is always required, even if there is no extra data after the payload.
The DataLength **MUST NOT** span over multiple headers.

### Data

This is the Data that has been encoded in the image.
The encoder and decoder **MUST NOT** transform the data in any way and always treat it as a sequence of octets.

# Multiple Files

Each image can store one pixel mode file.
Some images allow for multiple custom headers.
In this case, each header can contain a single file.

The format does not supports path names.
If an encoder wants to store entire directory structures,
it's recommend to store the structure in an existing file container format (zip, tar, 7z, etc.) and then store the single container file in the image.

# Encryption

Encryption is not part of the standard.

Fcfh implements [ACRYPT](https://github.com/AyrA/crypt) as an example.

This is in no way an indication that an encoder or decoder must implement the same algorithm.
There are enough tools that properly encrypt and decrypt files already.

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
- [X] GUI
- [X] Publish (some sort of) standard
