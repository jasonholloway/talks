#!/bin/bash -e

L='stdbuf -eL -oL'

trap 'kill $(jobs -p)' SIGINT

while true
do
  docker ps -qa
	sleep 0.3s
done \
	| awk '{ fresh=!v[$0]; v[$0]=1; if(fresh) { print $0; fflush() } }' \
	| while read -r cid
		do
			export i=$(( (i + 1) % 5 ))
      col=$((i + 31))			

		  docker logs -f $cid \
			  | xargs -L1 \
        | awk -v \
						cid=$cid -v col=$col \
						'{print "\033["col"m" substr(cid, 0, 6) ":\033[0m  " $0; fflush()}' &
    done

wait
