#!/bin/bash

CMD=$1

CUR_DIR=$(dirname $(readlink -f $0))
PID_FILE=$CUR_DIR/login.pid

function start(){
	rm -rf $CUR_DIR/log/login.log
	echo "start login service"
	$CUR_DIR/skynet/skynet $CUR_DIR/config/config.login
}

function stop(){
	if [ ! -f $PID_FILE ] ;then
		exit 0
	fi
	PID=$(cat $PID_FILE)
	PID_EXIST=$(ps aux | awk '{print $2}'| grep -w $PID)
	if [ ! $PID_EXIST ] ;then
		exit 0
	else
		echo "stop login service"
		kill -9 $PID
	fi
}

case "$CMD" in
	start )
		start
		;;
	stop )
		stop
		;;
	*)
	exit 2
esac