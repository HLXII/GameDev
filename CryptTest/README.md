# CryptTest

iOS Puzzle game dealing with a grid of "gems".

Each gem will have two main parts: The main gem, and rim gems.

The goal of every puzzle will be to match the main gem color with the rim gem colors.

## Puzzle Types

### Lights

By tapping a gem, the gem and it's cardinal neighbors will have their main gem color flipped. This game type only uses two different colors for flipping. To make it more interesting, a rotation of colors could be used, however it may be a little too complicated then.

### Rows and Columns

By dragging a gem up/down or left/right, the main gem colors of the current column/row will be rotated, looping around the edges of the board. 

### Inverted Dragging

Dragging over multiple gems creates a line of selected gems. Once the dragging is released, the colors of the gems selected will be reversed and replaced. Thus, the first gem selected will swap its color with the last gem, and so on.

### 3x3 Switching

Tapping two separate gems will swap the 3x3 grids they are in.

### 3x3 Rotation

Tapping a gem will rotate its surrounding neighbors' colors

### Switching Dragging

Dragging over multiple gems creates a line of selected gems. Once the dragging is released, the colors are switched using a preset rotation of colors.

