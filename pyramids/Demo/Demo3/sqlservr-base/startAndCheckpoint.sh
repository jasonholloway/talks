#!/bin/bash
set -e

main() {
	bumpPids 20

	sqlservr -x &
	sleep 20
	checkpoint $!
}

# dumps everything related to the given pid to the filesystem
# ready for fast restoration
checkpoint() {
	echo -n "$1" > sqlservr.pid
	criu dump \
		-t "$1" \
		--shell-job \
		--ghost-limit 100000000
}

# sqlservr needs to have a big pid on restore to allow us wriggle room
# hence this loop of pointless forks, each one incrementing pid for subsequent processes
bumpPids() {
	for i in $(seq 0 $1); do
		echo "boo" 1>/dev/null &
	done
}

main
