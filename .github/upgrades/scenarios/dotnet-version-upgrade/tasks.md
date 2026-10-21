# .NET 10 Upgrade Progress

## Overview

Upgrading Depends.Core, Snitch, and Snitch.Tests from net8.0 to net10.0 using an All-At-Once strategy, including the global.json SDK pin and vulnerable/deprecated packages.

**Progress**: 2/3 tasks complete <progress value="67" max="100"></progress> 67%

## Tasks

- ✅ 01-prerequisites: Verify .NET 10 SDK and update global.json ([Content](tasks/01-prerequisites/task.md), [Progress](tasks/01-prerequisites/progress-details.md))
- ✅ 02-upgrade-all-projects: Retarget all projects to net10.0 and update packages ([Content](tasks/02-upgrade-all-projects/task.md), [Progress](tasks/02-upgrade-all-projects/progress-details.md))
- 🔄 03-final-validation: Build and test the full solution ([Content](tasks/03-final-validation/task.md))
