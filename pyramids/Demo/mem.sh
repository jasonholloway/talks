#!/bin/bash -e

statsFile=stats.csv

echo 'time,%cpu,%mem'>$statsFile

trap 'kill %1' SIGINT
kst2 $statsFile -n 100 -s 1 -x 1 -y 2 -y 3 &


tm0=$(date +%s%1N)

while true; do
  pgrep sqlservr \
    | awk '{print "-p "$1}' \
    | xargs top -b -d0.05 -n1 \
    | tail -n2 \
    | awk -v tm=$(date +%s%1N) -v tm0=$tm0 '{cpu+=$9;mem+=$10}END{print ((tm-tm0)/10) ", "cpu", "mem}' \
    | tee -a $statsFile \
  || break
done
