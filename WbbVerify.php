<?php
/*
*	This File is under the license of Creative Commons BY-NC 3.0 AT
*	Author: Thomas Marangoni/DasChaos
*	Created: 26.01.2018
*
*	You are only allowed to use this file in non-commercial way.
*	If you are using this file, you have to mark the author, add a link to the License
*	and announce any changes. 
*
*	https://creativecommons.org/licenses/by-nc/3.0/at/
*
*/

require_once('global.php');
use wcf\data\user\User;
use wcf\data\user\group\UserGroup;

$code = 0;
$json = ["StatusCode" => $code, "UserData" => null];
checkPassword($_POST['Username'], $_POST['Password'], $_POST['Key']);

function checkPassword($username, $password, $key) {
	$secretKey = ""; //128-Character Key

	if (strcmp($key, $secretKey) != 0)
	{
		global $code;
		global $json;
		$code = 1;
		$json = ["statusCode" => $code, "userData" => null];
		return null;
	}
	if (empty($username) || empty($password)) {
		global $code;
		global $json;
		$code = 2;
		$json = ["statusCode" => $code, "userData" => null]; 
		return null;
	}
	$user = User::getUserByUsername($username);
	if(!$user->userID)
	{
		global $code;
		global $json;
		$code = 11;
		$json = ["statusCode" => $code, "userData" => null];
		return null;
	}
	else if (!$user->checkPassword($password)) {
		global $code;
		global $json;
		$code = 11;
		$json = ["statusCode" => $code, "userData" => null];
		return null;
	}
	else {
		global $code;
		global $json;
		$whitelisted = in_array(10 ,$user->getGroupIDs()); //Replace 10 with Group ID which is for whitelist
		$code = 10;
		$json = ["statusCode" => $code, "userData" =>  ["userId" => $user->userID, "username" => $user->username, "banned" => (bool)$user->banned, "banReason" => $user->banReason, "whitelisted" => (bool)$whitelisted]];
		return null;
	}
}

echo json_encode($json);
?>
