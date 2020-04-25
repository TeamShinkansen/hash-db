# hash-db

This repo contains a number of data files that map CRC32 hashes to the correspond IDs for TheGamesDB.

Inside, you'll find a tool that generates data files for Hakchi2 CE and also makes modifying these data files easier, it can also import standard .dat files and will automatically match up any existing games by their hash and remove them from the old data file, if no games are left in the old data file, that file will be deleted.
