// 初始化变量
var auctionDate = 0;
var _GMTEndTime = 0;
var showTime = "leftTime";
var _day = '天';
var _hour = '小时';
var _minute = '分钟';
var _second = '秒';
var _end = '结束';

var cur_date = new Date();
var startTime = cur_date.getTime();
var Temp;
var timerID = null;
var timerRunning = false;

function showtime(){
  now = new Date();
  var ts = parseInt((startTime - now.getTime()) / 1000) + auctionDate;
  var dateLeft = 0;
  var hourLeft = 0;
  var minuteLeft = 0;
  var secondLeft = 0;
  var hourZero = '';
  var minuteZero = '';
  var secondZero = '';
  if (ts < 0) {
    ts = 0;
    CurHour = 0;
    CurMinute = 0;
    CurSecond = 0;
  }
  else
  {
    dateLeft = parseInt(ts / 86400);
    ts = ts - dateLeft * 86400;
    hourLeft = parseInt(ts / 3600);
    ts = ts - hourLeft * 3600;
    minuteLeft = parseInt(ts / 60);
    secondLeft = ts - minuteLeft * 60;
  }

  if (hourLeft < 10){
    hourZero = '0';
  }
  if (minuteLeft < 10){
    minuteZero = '0';
  }
  if (secondLeft < 10){
    secondZero = '0';
  }

    //<font class=\"day\" >2</font>天<font class=\"hour\">23</font>小时<font class=\"minute\">0</font>分 <font class=\"seconds\">9.2</font>

  if (dateLeft > 0){
      Temp = "<font class=\"day\" >" + dateLeft + "</font>" + _day + "<font class=\"hour\">" + hourZero + hourLeft + "</font>" + _hour + "<font class=\"minute\">" + minuteZero + minuteLeft + "</font>" + _minute + "<font class=\"seconds\">" + secondZero + secondLeft + "</font>" + _second;
  }
  else{
    if (hourLeft > 0){
        Temp = "<font class=\"hour\">" + hourLeft + "</font>" + _hour + "<font class=\"minute\">" + minuteZero + minuteLeft + "</font>" + _minute + "<font class=\"seconds\">" + secondZero + secondLeft + "</font>" + _second;
    }
    else{
      if (minuteLeft > 0){
          Temp = "<font class=\"minute\">" + minuteLeft + "</font>" + _minute + "<font class=\"seconds\">" + secondZero + secondLeft + "</font>" + _second;
      }
      else{
        if (secondLeft > 0){
            Temp = "<font class=\"seconds\">" + secondLeft + "</font>" + _second;
        }
        else{
          Temp = '';
        }
      }
    }
  }

  if (auctionDate <= 0 || Temp == ''){
    Temp = "<strong>" + _end + "</strong>";
    stopclock();
  }

  if (document.getElementById(showTime)){
    document.getElementById(showTime).innerHTML = Temp;
  }

  timerID = setTimeout("showtime()", 1000);
  timerRunning = true;
}

var timerID = null;
var timerRunning = false;
function stopclock(){
  if (timerRunning){
    clearTimeout(timerID);
  }
  timerRunning = false;
}

function macauclock(){
  stopclock();
  showtime();
}

function onload_leftTime(now_time){
    try {
    _GMTEndTime = gmt_end_time;
    // 剩余时间
    _day = day;
    _hour = hour;
    _minute = minute;
    _second = second;
    _end = end;
  }
  catch (e){
  }
  if (_GMTEndTime > 0){
      if (now_time == undefined){
        var tmp_val = parseInt(_GMTEndTime); 
    }
    else {
        var tmp_val = parseInt(_GMTEndTime) - now_time;
    }
    if (tmp_val > 0){
      auctionDate = tmp_val;
    }
  }

  macauclock();
  try{
    initprovcity();
  }
  catch (e){  }
}
