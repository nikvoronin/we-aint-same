# We Ain't Same

Let's find out duplicate images with **Perceptual Hashing** algorithms.

- Calculate perceptual hashes using `ImageHash` library.
- Store and restore precalculated hashes.
- Recursive seeking of image files.
- Detect duplicates.
- Organize images into the groups of duplicates.

## Docs

- [ImageHash](https://github.com/coenm/ImageHash) is a .NET Standard library containing multiple algorithms to calculate perceptual hashes of images and to calculate similarity using those hashes.
- [ImageSharp](https://github.com/SixLabors/ImageSharp) is used inside of `ImageHash`.
- [Detection of Duplicate Images Using Image Hash Functions.](https://towardsdatascience.com/detection-of-duplicate-images-using-image-hash-functions-4d9c53f04a75) Automate the search for (near-)identical photos with the Python library undouble.
