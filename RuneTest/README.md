# RuneTest

Testing a simulated circuit-like implementation of runes in an action/puzzle RPG. 

## Runes

There will be 3 different rune types: Triangle, Square, and Hexagon

The game will start off using Triangular Runes, as with a maximum of 3 ports, they are the simplest.

As the game progresses, Square and Hexagonal Runes will be necessary for higher power attacks as well as more complex puzzles.

### Planned Runes

At the moment, there aren't that many. There will need to be a lot to deal with all the possible puzzles.

#### Wires

The energy has to flow between circuit elements. Wire runes will allow for that. Each wire has an efficiency, which causes some of the energy flowing through to be disappated.

Wires also have a max capacity, and when overflowed, will destroy the rune.

Still undecided is whether wires can split energy into two paths, or whether energy flowing into a 2-port wire from both ports destroys the rune

#### Source Runes

Source runes bring energy to the circuit. These rune will be the ones turned on when the circuit is activated by the player. Different runes have different intake rates.

Still undecided is whether a certain amount of back current will destroy the rune or not.

#### Sink Runes

Sink Runes only disappate the energy inputted. They also have a certain capacity and disappation rate. If the intake is too high for the disappation rate, and the capacity is overloaded, the rune will break.

#### Output Runes

The main rune types. Much like Sink Runes, however their output creates effects in the overworld. A generic output rune would probably create a magic missile attack or something. More specialized output runes will have different overworld effects, which is important for puzzles or unique attacks. 

#### Transformer Runes

Runes that take in energy and transform it in some way. The main runes of this type would be 2-port runes which take in energy, and output the energy with some kind of elemental value. This would be used in created elemental attacks. 

## Puzzles

The main puzzles of the game will be attempting to solve problems using a runic array. This section contains all the potential puzzles so far.

There are none LOL.

There will be a training area somewhere to help with understanding how runes work. The puzzles there will be simple to help the player create their own runic arrays. Something like creating an ordinary fireball spell.

## Gameplay

This may be part of a larger RPG (It probably will be, but since I'm never finishing anything, it doesn't matter).

As another gameplay mechanic to deal damage, weapons or armor must be equipped that can hold a runic array. The weapons and armor will all be randomly generated, and some will have rune slots. These runic matrices will have random shapes which makes the player have to implement their circuit with size constraints. For larger weapons, the larger the runic matrix will be.