# .NET 10 Upgrade Progress

## Overview

Upgrading Depends.Core, Snitch, and Snitch.Tests from net8.0 to net10.0 using an All-At-Once strategy, including the global.json SDK pin and vulnerable/deprecated packages.

**Progress**: 0/3 tasks complete <progress value="0" max="100"></progress> 0%

## Tasks

- 🔄 01-prerequisites: Verify .NET 10 SDK and update global.json ([Content](tasks/01-prerequisites/task.md))
- 🔲 02-upgrade-all-projects: Retarget all projects to net10.0 and update packages
- 🔲 03-final-validation: Build and test the full solution
