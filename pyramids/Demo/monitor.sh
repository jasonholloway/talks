#!/bin/bash

sess=monitor

tmux kill-session -t monitor

tmux -2 new-session -d -s $sess

tmux new-window -t $sess:1 -n 'docker ps'
sleep 0.5s
tmux send-keys 'docker ps -a' C-m

tmux new-window -t $sess:2 -n 'hello'
sleep 0.5s
tmux send-keys -t $sess:2 'echo poo!' C-m



tmux -2 attach-session -t $sess

