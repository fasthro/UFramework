#!/bin/bash
protos=$(ls ../Protos)

for proto in $protos
do
	echo $proto

	protodir=../Protos/
	protopath=${protodir}${proto}

	pbdir=./src/proto/
	pbname=${proto%.*}
	pbext=.pb
	pbpath=${pbdir}${pbname}${pbext}

	protoc -o $pbpath $protopath -I=${protodir}
done