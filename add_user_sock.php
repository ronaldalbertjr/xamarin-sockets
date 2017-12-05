<?php
	error_reporting(E_ALL);

	set_time_limit(0);
	ob_implicit_flush();
	$address = '192.168.0.105';
	$port = 8880;


	if (($sock = socket_create(AF_INET, SOCK_STREAM, SOL_TCP)) === false) 
	{
    	echo "socket_create() failed: reason: " . socket_strerror(socket_last_error()) . "\n";
	}
	if (socket_bind($sock, $address, $port) === false) 
	{
    	echo "socket_bind() failed: reason: " . socket_strerror(socket_last_error($sock)) . "\n";
	}
	if (socket_listen($sock, 5) === false) 
	{
    	echo "socket_listen() failed: reason: " . socket_strerror(socket_last_error($sock)) . "\n";
	}

	do {
    	if (($msgsock = socket_accept($sock)) === false) 
    	{
        	echo "socket_accept() failed: reason: " . socket_strerror(socket_last_error($sock)) . "\n";
        	break;
    	}
   	 
    	$msg = "\nWelcome to the PHP Test Server. \n"; 
    	socket_write($msgsock, $msg, strlen($msg));

    	do 
    	{
        	if (false === ($buf = socket_read($msgsock, 2048, PHP_NORMAL_READ))) 
        	{
            	echo "socket_read() failed: reason: " . socket_strerror(socket_last_error($msgsock)) . "\n";
            	break 2;
        	}
        	$values = "'".implode("', '", explode('|', $buf))."'";
        	$conn = mysqli_connect(localhost, root, null, sockets) or die(mysqli_connect_error());
        	$query = "INSERT INTO user (login, password) VALUES ({$values})";
        	$result =  mysqli_query($conn, $query) or die(mysqli_error($link));

        	if($result === false)
        	{
        		$talkback = "The connection failed";
        	}
        	else
        	{
        		$talkback = "User successfully registered"
        	}
        	socket_write($msgsock, $talkback, strlen($talkback));
    	} while (true);
    socket_close($msgsock);
	} while (true);
	socket_close($sock);
?>