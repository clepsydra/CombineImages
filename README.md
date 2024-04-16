# CombineImages
## Description
This tool can be used to combine two pictures into one.

## Usage
It takes a directory as parameter and reads all `.jpg` files.

It then groups them by Width/Height ratio.

It then combines pairs of pictures with the same ratio into one and stores the resulting picture in the `Out` subdirectory.

## Intention
I had the problem that I needed small pictures for a personal project.
I wanted to get them printed in a store so that the quality is good.
But the store only offered as a minimum size 9cm for the smaller side.

To address the problem I looked for a tool to combine two pictures into one, but did not quickly find one.

So I created this tool.