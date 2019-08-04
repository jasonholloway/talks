#!/bin/bash
set -e

main() {
	(buildBase sqlservr-base)
	(buildDerived sqlservr-tracking)
	echo "Built super fast sqlservr-tracking image"
}

buildBase() {
	image=$1
	cd "$image"
	docker build -t "$image" .
}

buildDerived() {
	image=$1
	cd "$image"

	old=$(docker image ls -q "$image" || true)

	temp="$image-tmp-$(uuidgen)"
	docker build -t "$temp" .

	cont=$(docker run -d \
            --network=host \
            --privileged \
            --security-opt seccomp=unconfined \
            --tmpfs=/run \
            "$temp" \
            ./startAndCheckpoint.sh) 
            
	docker logs -f "$cont" &
	docker wait "$cont"
	
	docker commit \
		-c 'CMD ["./start.sh"]' \
		"$cont" \
		"$image" 

	docker rm -f "$cont"
	docker image rm -f "$temp" "$old"
	wait
}

main
