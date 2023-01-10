<?php 
class final_rest
{



/**
 * @api  /api/v1/setTemp/
 * @apiName setTemp
 * @apiDescription Add remote temperature measurement
 *
 * @apiParam {string} location
 * @apiParam {String} sensor
 *
 * @apiSuccess {Integer} status
 * @apiSuccess {string} message
 *
 * @apiSuccessExample Success-Response:
 *     HTTP/1.1 200 OK
 *     {
 *              "status":0,
 *              "message": ""
 *     }
 *
 * @apiError Invalid data types
 *
 * @apiErrorExample Error-Response:
 *     HTTP/1.1 200 OK
 *     {
 *              "status":1,
 *              "message":"Error Message"
 *     }
 *
 */
	public static function setTemp ($date, $location, $low, $high, $forecast)
	{
			try {
				EXEC_SQL("insert into temperature (daterequested, date, location, low, high, forecast) values (CURRENT_TIMESTAMP,?,?,?,?,?)",$date, $location, $low, $high, $forecast);
				$retData["status"]=0;
				$retData["message"]="insert of low:'$low', high:'$high'  for location: '$location' and forcast: '$forecast' on '$date'";
				
			}
			catch  (Exception $e) {
				$retData["status"]=1;
				$retData["message"]=$e->getMessage();
			}

		return json_encode ($retData);
	}


	public static function getTemp($date, $sort) 
	{
		try {
			   $retData["status"]=0;
			   $retData["message"]= "select * from temperature where date = '$date' and sort = '$sort'"; 
			   if ($sort == 1) {
  				$retData["result"] = GET_SQL("select * from temperature where date=? order by location ,dateRequested", $date);
			   } elseif ($sort == 2) {
				$retData["result"] = GET_SQL("select * from temperature where date=? order by dateRequested,location ", $date);
			   } 
		}
		catch (Exception $e) {
			$retData["status"]=1;
			$retData["message"]=$e->getMessage();
		}
	       return json_encode ($retData);
	}
}


