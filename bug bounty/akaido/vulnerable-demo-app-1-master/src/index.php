<?php

function getFilesize($filename) {
	$size = filesize($filename);
	return $size;
}

$filesize = getFilesize($_POST["file"]);
