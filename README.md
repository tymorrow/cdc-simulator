CDC Timing Simulation
=============

This is a timing calculator that simulates the expected timing of instruction execution in the both the CDC 6600 and CDC 7600.

This application emulates the processors of both machines in an object-oriented fashion and sequentially feeds instructions to them, not unlike the actual instruction pipeline.

One of the biggest difference between the two machines is that the CDC 6600 does not have pipelining ALU modules, whereas the CDC 7600 does.
This, along with numerous other improvements, end up showing an increase in speed for the same instruction sets given to both machines.

This was a project for my High Performance Computer Architecture course; it is compatible with development on Windows & Linux (with Mono).
