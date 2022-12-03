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
+ C:\Users\Pictures\Samples\Pictures\bing20221129.jpg
+ C:\Users\Pictures\Samples\Pictures\fireworks.jpg
+ C:\Users\Pictures\Samples\Pictures\mars.jpg
+ C:\Users\Pictures\Samples\Pictures\mount-copy.jpg
+ C:\Users\Pictures\Samples\Pictures\mount-rotated-2degree.jpg
+ C:\Users\Pictures\Samples\Pictures\mount-small.jpg
+ C:\Users\Pictures\Samples\Pictures\mountains.jpg

+++ Chasing duplicates...
....>>> mount-copy.jpg
        dup: 90,625% <> mount-rotated-2degree.jpg
>>> mount-copy.jpg
        dup: 100% <> mount-small.jpg
>>> mount-copy.jpg
        dup: 100% <> mountains.jpg
...
+++ Similarity: max= 100% / min= 40,625%

+++ Duplicate Groups (1):
Group #1
        mount-copy.jpg
        mount-rotated-2degree.jpg
        mount-small.jpg
        mountains.jpg

+++ TOTAL: 00:00:00.7362885
```

Precalculated hashes as JSON file:

```json
[
    {
        "Path": "C:\\Users\\Pictures\\Samples\\Pictures\\bing20221129.jpg",
        "Hash": 11695141823225099355
    },
    {
        "Path": "C:\\Users\\Pictures\\Samples\\Pictures\\fireworks.jpg",
        "Hash": 10721035060630703339
    },
    // ...
]
```

## Links

- [ImageHash](https://github.com/coenm/ImageHash) is a .NET Standard library containing multiple algorithms to calculate perceptual hashes of images and to calculate similarity using those hashes.
- [ImageSharp](https://github.com/SixLabors/ImageSharp) is used inside of `ImageHash`.
- [Detection of Duplicate Images Using Image Hash Functions.](https://towardsdatascience.com/detection-of-duplicate-images-using-image-hash-functions-4d9c53f04a75) Automate the search for (near-)identical photos with the Python library undouble.
