###############################################################################
# Constants
###############################################################################

$defaultPort = 8801
$defaultSiteName = "Oracle Fiddler ($defaultPort)"

$defaultDbServer = "oxe"
$defaultDb = "XE"
$defaultDbUser = "OraPlant"
$defaultDbPassword = "OraPlant"

$defaultShowAspNetErrors = $false

$iis = "$env:windir\system32\inetsrv\appcmd.exe"

###############################################################################
# Functions
###############################################################################

function readInput ($message, $defaultValue) {
	$value = Read-Host "$message (default is $defaultValue)"
	if (-not $value) {
		$value = $defaultValue
	}
	return $value
}

function anyKeyTo ($message) {
	"Press any key to $message..."
	$notUsed = $Host.UI.RawUI.ReadKey()
}

function deleteSite ($siteName) {
	& $iis delete site $siteName
}

function addSite ($siteName, $port, $path) {
	& $iis add site /name:$siteName /id:$port /physicalPath:"$path" /bindings:"http/*:$port":
}

function enable32bitAppsOnDefaultAppPool { # think about extracting app pool name as parameter
	& $iis set apppool DefaultAppPool -enable32BitAppOnWin64:True
}

function openWebConfig ($path) {
	[xml] $config = Get-Content $path
	return $config
}

function saveWebConfig ($config, $path) {
	$config.Save($path)
}

function setConnectionString ($config, $connectionString) {
	$config.SelectSingleNode("//connectionStrings/add[name='default']).connectionString = $connectionString
}

function showAspNetErrors ($config, $show = $true) {
	$node = $config.SelectSingleNode("//system.web/customErrors")
	if ($show) {
		$node.mode = "Off"
	}
	else {
		$node.mode = "RemoteOnly"
	}
}

###############################################################################
# Script itself
###############################################################################

$startTime = Get-Date
"Started on $startTime"

$port = readInput "Port" $defaultPort
$siteName = readInput "Site name" $defaultSiteName

$baseDir = Resolve-Path .

"Deleting previous installation if any..."
deleteSite $siteName

"Installing..."
addSite $siteName $port $baseDir

"Configuring..."
$configPath = Join-Path $baseDir "web.config"
$config = openWebConfig $configPath
$dbServer = readInput "DB server" $defaultDbServer
$db = readInput "DB" $defaultDb
$dbUser = readInput "DB user" $defaultDbUser
$dbPassword = readInput "DB password" $defaultDbPassword
setConnectionString $config "Data Source=$dbServer/$db;User ID=$dbUser;Password=$dbPassword"
$showAspNetErrors = readInput "Show ASP.NET errors" $defaultShowAspNetErrors
showAspNetErrors $config $showAspNetErrors
saveWebConfig $config $configPath

$finishTime = Get-Date
"Finished on $finishTime"
"Time taken: {0}" -f ($finishTime - $startTime)

anyKeyTo "exit"