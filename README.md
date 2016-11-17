CDC Timing Simulation
=============

This is a timing calculator that simulates the expected timing of instruction execution in the both the CDC 6600 and CDC 7600.

This application emulates the processors of both machines in an object-oriented fashion and sequentially feeds instructions to them, not unlike the actual instruction pipeline.

One of the biggest differences between the two machines is that the CDC 6600 does not have ALU modules that support pipelining, whereas the CDC 7600 does.
This, along with some other improvements, results in the CDC 7600 processing an instruction set more quickly than the CDC 6600 assuming the chosen instruction set is suited to take advantage of pipelining.
In general, the CDC 7600 is faster.

This was a project for my High Performance Computer Architecture course; it is compatible with development on Windows & Linux (with Mono).
