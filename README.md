# We Ain't Same

Let's find out duplicate images with **Perceptual Hashing** algorithms.

- Calculate perceptual hashes using `ImageHash` library.
- Store and restore precalculated hashes.
- Recursive seeking of image files.
- Detect duplicates.
- Organize images into the groups of duplicates.

## Example

![samples002](https://user-images.githubusercontent.com/11328666/205454362-1e3044b4-92fb-4805-9e9b-bac5bcfb31f3.png)

Output log:

```plain
+++ Computing hashes...
+ C:\Users\Pictures\Samples2\bing20221129.jpg
+ C:\Users\Pictures\Samples2\fireworks.jpg
+ C:\Users\Pictures\Samples2\mars.jpg
+ C:\Users\Pictures\Samples2\mount-copy.jpg
+ C:\Users\Pictures\Samples2\mount-rotated-2degree.jpg
+ C:\Users\Pictures\Samples2\mount-small.jpg
+ C:\Users\Pictures\Samples2\mountains.jpg

+++ Chasing duplicates...

+++ Similarity: max= 100% / min= 43,75%

+++ Duplicate Groups (1):
Group #0
        mount-copy.jpg
        mount-rotated-2degree.jpg
        mount-small.jpg
        mountains.jpg

+++ TOTAL: 00:00:00.8170144
```

Precalculated hashes as JSON file:

```json
[
    {
        "Path": "C:\\Users\\Pictures\\Samples2\\bing20221129.jpg",
        "Hash": 11695141823225099355
    },
    {
        "Path": "C:\\Users\\Pictures\\Samples2\\fireworks.jpg",
        "Hash": 10721035060630703339
    },
    // ...
]
```

## Links

- [ImageHash](https://github.com/coenm/ImageHash) is a .NET Standard library containing multiple algorithms to calculate perceptual hashes of images and to calculate similarity using those hashes.
- [ImageSharp](https://github.com/SixLabors/ImageSharp) is used inside of `ImageHash`.
- [Detection of Duplicate Images Using Image Hash Functions.](https://towardsdatascience.com/detection-of-duplicate-images-using-image-hash-functions-4d9c53f04a75) Automate the search for (near-)identical photos with the Python library undouble.
