<?php

$dsn = 'mysql://root:password@localhost/symfony/';
$clients = [];
$auth_key = "d4349b94516a530a74ecba09662fb225382fbed89d0dd141858d6ffceb1fde76";

if (strcmp(PHP_SAPI, 'cli') === 0)
{
	exit('ArrestDB should not be run from CLI.' . PHP_EOL);
}

if ((empty($clients) !== true) && (in_array($_SERVER['REMOTE_ADDR'], (array) $clients) !== true))
{
	exit(ArrestDB::Reply(ArrestDB::$HTTP[403]));
}

else if (ArrestDB::Query($dsn) === false)
{
	exit(ArrestDB::Reply(ArrestDB::$HTTP[503]));
}

if (array_key_exists('_method', $_GET) === true)
{
	$_SERVER['REQUEST_METHOD'] = strtoupper(trim($_GET['_method']));
}

else if (array_key_exists('HTTP_X_HTTP_METHOD_OVERRIDE', $_SERVER) === true)
{
	$_SERVER['REQUEST_METHOD'] = strtoupper(trim($_SERVER['HTTP_X_HTTP_METHOD_OVERRIDE']));
}

function verifyAuth(){
	global $auth_key;

	if (isset($_GET['auth_key']))
	{
		if($auth_key != $_GET['auth_key'])
		{
			return false;
		}
		return true;
	}
	else
	{
		return false;
	}	
}

ArrestDB::Serve('GET', '/login/(#any)/(#any)', function ($login, $password)
{
	if(verifyAuth())
	{
		// ArrestDB ne voulant pas faire la query (retourne systématiquement un False -> Passage en PDO)
		// Connect to the database
		$uri = 'mysql:host=localhost;dbname=symfony';
		$bdd_username = 'root';
		$bdd_password = 'password';
		$options = array(
		    PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES utf8',
		);

		$pdo = new PDO($uri, $bdd_username, $bdd_password, $options);

		//Verify User Existance
		$query = array
		(
			sprintf("SELECT salt, password FROM Compte"),
			sprintf("WHERE `username`='%s'", $login),
		);
		$query = sprintf('%s;', implode(' ', $query));

		$sth = $pdo->prepare($query);
		$sth->execute();
		$result = $sth->fetchAll();

		if ($result === false)
		{
			$result = ArrestDB::$HTTP[404];
		}

		else if (empty($result) === true)
		{
			$result = ArrestDB::$HTTP[204];
		}

		else
		{
			// Save only the first result
			$result = $result[0];

			//Get Data
			$salt = $result["salt"];
			$hashedpassword = $result["password"];

			//GenerateHashedPassword
			$salted = $password.'{'.$salt.'}';
			$encryptpassword = hash("sha512", $salted, true);

			for ($i = 1; $i < 5000; $i++) {
	            $encryptpassword = hash("sha512", $encryptpassword.$salted, true);
	        }

	        $encryptpassword = base64_encode($encryptpassword);
	        $result = null;

			//VerifyPassword
			if($encryptpassword == $hashedpassword)
			{
				$result = ArrestDB::$HTTP[200];
			}
			else
			{
				$result = ArrestDB::$HTTP[204];
			}
		}

		return ArrestDB::Reply($result);
	}
	else
	{
		$result = ArrestDB::$HTTP[403];
		return ArrestDB::Reply($result);
	}
});

ArrestDB::Serve('GET', '/(#any)/(#any)/(#any)', function ($table, $id, $data)
{
	if(verifyAuth())
	{
		$query = array
		(
			sprintf('SELECT * FROM "%s"', $table),
			sprintf('WHERE "%s" %s ?', $id, (ctype_digit($data) === true) ? '=' : 'LIKE'),
		);

		if (isset($_GET['by']) === true)
		{
			if (isset($_GET['order']) !== true)
			{
				$_GET['order'] = 'ASC';
			}

			$query[] = sprintf('ORDER BY "%s" %s', $_GET['by'], $_GET['order']);
		}

		if (isset($_GET['limit']) === true)
		{
			$query[] = sprintf('LIMIT %u', $_GET['limit']);

			if (isset($_GET['offset']) === true)
			{
				$query[] = sprintf('OFFSET %u', $_GET['offset']);
			}
		}

		$query = sprintf('%s;', implode(' ', $query));
		$result = ArrestDB::Query($query, $data);

		if ($result === false)
		{
			$result = ArrestDB::$HTTP[404];
		}

		else if (empty($result) === true)
		{
			$result = ArrestDB::$HTTP[204];
		}

		return ArrestDB::Reply($result);
	}
	else
	{
		$result = ArrestDB::$HTTP[403];
		return ArrestDB::Reply($result);
	}
});

ArrestDB::Serve('GET', '/(#any)/(#num)?', function ($table, $id = null)
{
	if(verifyAuth())
	{
		$query = array
		(
			sprintf('SELECT * FROM "%s"', $table),
		);

		if (isset($id) === true)
		{
			$query[] = sprintf('WHERE "%s" = ? LIMIT 1', 'id');
		}

		else
		{
			if (isset($_GET['by']) === true)
			{
				if (isset($_GET['order']) !== true)
				{
					$_GET['order'] = 'ASC';
				}

				$query[] = sprintf('ORDER BY "%s" %s', $_GET['by'], $_GET['order']);
			}

			if (isset($_GET['limit']) === true)
			{
				$query[] = sprintf('LIMIT %u', $_GET['limit']);

				if (isset($_GET['offset']) === true)
				{
					$query[] = sprintf('OFFSET %u', $_GET['offset']);
				}
			}
		}

		$query = sprintf('%s;', implode(' ', $query));
		$result = (isset($id) === true) ? ArrestDB::Query($query, $id) : ArrestDB::Query($query);

		if ($result === false)
		{
			$result = ArrestDB::$HTTP[404];
		}

		else if (empty($result) === true)
		{
			$result = ArrestDB::$HTTP[204];
		}

		else if (isset($id) === true)
		{
			$result = array_shift($result);
		}

		return ArrestDB::Reply($result);
	}
	else
	{
		$result = ArrestDB::$HTTP[403];
		return ArrestDB::Reply($result);
	}
});


exit(ArrestDB::Reply(ArrestDB::$HTTP[400]));

class ArrestDB
{
	public static $HTTP = [
		200 => [
			'success' => [
				'code' => 200,
				'status' => 'OK',
			],
		],
		201 => [
			'success' => [
				'code' => 201,
				'status' => 'Created',
			],
		],
		204 => [
			'error' => [
				'code' => 204,
				'status' => 'No Content',
			],
		],
		400 => [
			'error' => [
				'code' => 400,
				'status' => 'Bad Request',
			],
		],
		403 => [
			'error' => [
				'code' => 403,
				'status' => 'Forbidden',
			],
		],
		404 => [
			'error' => [
				'code' => 404,
				'status' => 'Not Found',
			],
		],
		409 => [
			'error' => [
				'code' => 409,
				'status' => 'Conflict',
			],
		],
		503 => [
			'error' => [
				'code' => 503,
				'status' => 'Service Unavailable',
			],
		],
	];

	public static function Query($query = null)
	{
		static $db = null;
		static $result = [];

		try
		{
			if (isset($db, $query) === true)
			{
				if (strncasecmp($db->getAttribute(\PDO::ATTR_DRIVER_NAME), 'mysql', 5) === 0)
				{
					$query = strtr($query, '"', '`');
				}

				if (empty($result[$hash = crc32($query)]) === true)
				{
					$result[$hash] = $db->prepare($query);
				}

				$data = array_slice(func_get_args(), 1);

				if (count($data, COUNT_RECURSIVE) > count($data))
				{
					$data = iterator_to_array(new \RecursiveIteratorIterator(new \RecursiveArrayIterator($data)), false);
				}

				if ($result[$hash]->execute($data) === true)
				{
					$sequence = null;

					if ((strncmp($db->getAttribute(\PDO::ATTR_DRIVER_NAME), 'pgsql', 5) === 0) && (sscanf($query, 'INSERT INTO %s', $sequence) > 0))
					{
						$sequence = sprintf('%s_id_seq', trim($sequence, '"'));
					}

					switch (strstr($query, ' ', true))
					{
						case 'INSERT':
						case 'REPLACE':
							return $db->lastInsertId($sequence);

						case 'UPDATE':
						case 'DELETE':
							return $result[$hash]->rowCount();

						case 'SELECT':
						case 'EXPLAIN':
						case 'PRAGMA':
						case 'SHOW':
							return $result[$hash]->fetchAll();
					}

					return true;
				}

				return false;
			}

			else if (isset($query) === true)
			{
				$options = array
				(
					\PDO::ATTR_CASE => \PDO::CASE_NATURAL,
					\PDO::ATTR_DEFAULT_FETCH_MODE => \PDO::FETCH_ASSOC,
					\PDO::ATTR_EMULATE_PREPARES => false,
					\PDO::ATTR_ERRMODE => \PDO::ERRMODE_EXCEPTION,
					\PDO::ATTR_ORACLE_NULLS => \PDO::NULL_NATURAL,
					\PDO::ATTR_STRINGIFY_FETCHES => false,
				);

				if (preg_match('~^sqlite://([[:print:]]++)$~i', $query, $dsn) > 0)
				{
					$options += array
					(
						\PDO::ATTR_TIMEOUT => 3,
					);

					$db = new \PDO(sprintf('sqlite:%s', $dsn[1]), null, null, $options);
					$pragmas = array
					(
						'automatic_index' => 'ON',
						'cache_size' => '8192',
						'foreign_keys' => 'ON',
						'journal_size_limit' => '67110000',
						'locking_mode' => 'NORMAL',
						'page_size' => '4096',
						'recursive_triggers' => 'ON',
						'secure_delete' => 'ON',
						'synchronous' => 'NORMAL',
						'temp_store' => 'MEMORY',
						'journal_mode' => 'WAL',
						'wal_autocheckpoint' => '4096',
					);

					if (strncasecmp(PHP_OS, 'WIN', 3) !== 0)
					{
						$memory = 131072;

						if (($page = intval(shell_exec('getconf PAGESIZE'))) > 0)
						{
							$pragmas['page_size'] = $page;
						}

						if (is_readable('/proc/meminfo') === true)
						{
							if (is_resource($handle = fopen('/proc/meminfo', 'rb')) === true)
							{
								while (($line = fgets($handle, 1024)) !== false)
								{
									if (sscanf($line, 'MemTotal: %d kB', $memory) == 1)
									{
										$memory = round($memory / 131072) * 131072; break;
									}
								}

								fclose($handle);
							}
						}

						$pragmas['cache_size'] = intval($memory * 0.25 / ($pragmas['page_size'] / 1024));
						$pragmas['wal_autocheckpoint'] = $pragmas['cache_size'] / 2;
					}

					foreach ($pragmas as $key => $value)
					{
						$db->exec(sprintf('PRAGMA %s=%s;', $key, $value));
					}
				}

				else if (preg_match('~^(mysql|pgsql)://(?:(.+?)(?::(.+?))?@)?([^/:@]++)(?::(\d++))?/(\w++)/?$~i', $query, $dsn) > 0)
				{
					if (strncasecmp($query, 'mysql', 5) === 0)
					{
						$options += array
						(
							\PDO::ATTR_AUTOCOMMIT => true,
							\PDO::MYSQL_ATTR_INIT_COMMAND => 'SET NAMES "utf8" COLLATE "utf8_general_ci", time_zone = "+00:00";',
							\PDO::MYSQL_ATTR_USE_BUFFERED_QUERY => true,
						);
					}

					$db = new \PDO(sprintf('%s:host=%s;port=%s;dbname=%s', $dsn[1], $dsn[4], $dsn[5], $dsn[6]), $dsn[2], $dsn[3], $options);
				}
			}
		}

		catch (\Exception $exception)
		{
			return false;
		}

		return (isset($db) === true) ? $db : false;
	}

	public static function Reply($data)
	{
		$bitmask = 0;
		$options = ['UNESCAPED_SLASHES', 'UNESCAPED_UNICODE'];

		if (empty($_SERVER['HTTP_X_REQUESTED_WITH']) === true)
		{
			$options[] = 'PRETTY_PRINT';
		}

		foreach ($options as $option)
		{
			$bitmask |= (defined('JSON_' . $option) === true) ? constant('JSON_' . $option) : 0;
		}

		if (($result = json_encode($data, $bitmask)) !== false)
		{
			$callback = null;

			if (array_key_exists('callback', $_GET) === true)
			{
				$callback = trim(preg_replace('~[^[:alnum:]\[\]_.]~', '', $_GET['callback']));

				if (empty($callback) !== true)
				{
					$result = sprintf('%s(%s);', $callback, $result);
				}
			}

			if (headers_sent() !== true)
			{
				header(sprintf('Content-Type: application/%s; charset=utf-8', (empty($callback) === true) ? 'json' : 'javascript'));
			}
		}

		return $result;
	}

	public static function Serve($on = null, $route = null, $callback = null)
	{
		static $root = null;

		if (isset($_SERVER['REQUEST_METHOD']) !== true)
		{
			$_SERVER['REQUEST_METHOD'] = 'CLI';
		}

		if ((empty($on) === true) || (strcasecmp($_SERVER['REQUEST_METHOD'], $on) === 0))
		{
			if (is_null($root) === true)
			{
				$root = preg_replace('~/++~', '/', substr($_SERVER['PHP_SELF'], strlen($_SERVER['SCRIPT_NAME'])) . '/');
			}

			if (preg_match('~^' . str_replace(['#any', '#num'], ['[^/]++', '[0-9]++'], $route) . '~i', $root, $parts) > 0)
			{
				return (empty($callback) === true) ? true : exit(call_user_func_array($callback, array_slice($parts, 1)));
			}
		}

		return false;
	}
}