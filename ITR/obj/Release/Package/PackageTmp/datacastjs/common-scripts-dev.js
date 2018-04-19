/*
 * jQuery JavaScript
 * Created by : VishnuVardhan Reddy Ammana & Raghuveer.B
 * Created Date: 04/March/2014
 * Note:  1) url--/ControllerName/ActionMethodName/   // ex: '/UserRole/Create/'
 *        2) obj  denotes the DataModel object  //Pass the Model while calling these scripts..   
 *        3) Build the modelobject in javascript like for Userrole Model
 *              var modelObj ={ 
 *               RoleName: $("#RoleName").val(),  
 *               RoleDescription: $("#RoleDescription").val()
 *                    };
 *         
 * Call these functions in your page like var result=SendRequestJSON(url,modelObj)
 * Handle the response as you needed.
 *
 *
 */



//Send an ajax request to Httphandler..receives string
this.SendRequestTEXT = function (url, obj) {
    // alert(obj);
    var response;
    $.ajax({
        type: "POST",
        async: false,
        url: url,
        dataType: "text",
        data: obj,
        success: function (res) {
            response = res;
        },
        error: function () {
            throw new Error("Error performing SendRequestTEXT");
        }
    });
    return response;
}

//Send an ajax request to Httphandler..receives a JSON object

this.SendRequestJSON = function (url, modelobj) {
    var response;
    $.ajax({
        type: "POST",
        async: false,
        url: url,
        dataType: "json",
        data: modelobj, // like for UserRole-Model {RoleName:obj.RoleName,RoleDescription:obj.RoleDescription},
        success: function (res) {
            response = res;
        },
        error: function () {
            //  throw new Error
            $('#loadingDiv .bg').height('100%');
            $('#loadingDiv').remove();
            $('body').css('cursor', 'default');
            //throw new Error
        }
    });
    return response;
}

//Send an ajax request to Httphandler..receives a JSON object
this.SendRequestJSONCommand = function (url, elems, cmd) {
    var response;
    $.ajax({
        type: "POST",
        async: false,
        url: url,
        dataType: "json",
        data: { elements: JSON.stringify(elems), action: cmd },
        success: function (res) {
            response = res;
        },
        error: function () {
            throw new Error("Error performing SendRequestJSON");
        }
    });
    return response;
}

/* 
* Created by : VishnuVardhan Reddy Ammana
* Created Date: 22/May/2015
* For Displaying  Popup
*/

function ShowPopup() {
    $('#displayMessage').prepend('<span class="button b-close"><span>X</span></span>');//Prepending Close button to popup
    $('#displayMessage').bPopup({ autoClose: 2000 });// Triggering bPopup
    // $('#displayMessage').bPopup();// Triggering bPopup 
    // $("#displayMessage").fadeOut(2000); 
}
function ShowPopupRed() {//displays the error in red color
    $('#displayMessageRed').prepend('<span class="button b-close"><span>X</span></span>');//Prepending Close button to popup
    $('#displayMessageRed').bPopup({ autoClose: 2500 });// Triggering bPopup   
}
function SessionShowPopup() {

    $('#SessionPopup').bPopup();// Triggering bPopup 
}

////bPopup script Added By Vishnu, to overwrite regular browser alerts
//function RegisterPopupScript()
//{    
//    (function (c) { c.fn.bPopup = function (A, E) { function L() { a.contentContainer = c(a.contentContainer || b); switch (a.content) { case "iframe": var d = c('<iframe class="b-iframe" ' + a.iframeAttr + "></iframe>"); d.appendTo(a.contentContainer); t = b.outerHeight(!0); u = b.outerWidth(!0); B(); d.attr("src", a.loadUrl); l(a.loadCallback); break; case "image": B(); c("<img />").load(function () { l(a.loadCallback); F(c(this)) }).attr("src", a.loadUrl).hide().appendTo(a.contentContainer); break; default: B(), c('<div class="b-ajax-wrapper"></div>').load(a.loadUrl, a.loadData, function (d, b, e) { l(a.loadCallback, b); F(c(this)) }).hide().appendTo(a.contentContainer) } } function B() { a.modal && c('<div class="b-modal ' + e + '"></div>').css({ backgroundColor: a.modalColor, position: "fixed", top: 0, right: 0, bottom: 0, left: 0, opacity: 0, zIndex: a.zIndex + v }).appendTo(a.appendTo).fadeTo(a.speed, a.opacity); C(); b.data("bPopup", a).data("id", e).css({ left: "slideIn" == a.transition || "slideBack" == a.transition ? "slideBack" == a.transition ? f.scrollLeft() + w : -1 * (x + u) : m(!(!a.follow[0] && n || g)), position: a.positionStyle || "absolute", top: "slideDown" == a.transition || "slideUp" == a.transition ? "slideUp" == a.transition ? f.scrollTop() + y : z + -1 * t : p(!(!a.follow[1] && q || g)), "z-index": a.zIndex + v + 1 }).each(function () { a.appending && c(this).appendTo(a.appendTo) }); G(!0) } function r() { a.modal && c(".b-modal." + b.data("id")).fadeTo(a.speed, 0, function () { c(this).remove() }); a.scrollBar || c("html").css("overflow", "auto"); c(".b-modal." + e).unbind("click"); f.unbind("keydown." + e); k.unbind("." + e).data("bPopup", 0 < k.data("bPopup") - 1 ? k.data("bPopup") - 1 : null); b.undelegate(".bClose, ." + a.closeClass, "click." + e, r).data("bPopup", null); clearTimeout(H); G(); return !1 } function I(d) { y = k.height(); w = k.width(); h = D(); if (h.x || h.y) clearTimeout(J), J = setTimeout(function () { C(); d = d || a.followSpeed; var e = {}; h.x && (e.left = a.follow[0] ? m(!0) : "auto"); h.y && (e.top = a.follow[1] ? p(!0) : "auto"); b.dequeue().each(function () { g ? c(this).css({ left: x, top: z }) : c(this).animate(e, d, a.followEasing) }) }, 50) } function F(d) { var c = d.width(), e = d.height(), f = {}; a.contentContainer.css({ height: e, width: c }); e >= b.height() && (f.height = b.height()); c >= b.width() && (f.width = b.width()); t = b.outerHeight(!0); u = b.outerWidth(!0); C(); a.contentContainer.css({ height: "auto", width: "auto" }); f.left = m(!(!a.follow[0] && n || g)); f.top = p(!(!a.follow[1] && q || g)); b.animate(f, 250, function () { d.show(); h = D() }) } function M() { k.data("bPopup", v); b.delegate(".bClose, ." + a.closeClass, "click." + e, r); a.modalClose && c(".b-modal." + e).css("cursor", "pointer").bind("click", r); N || !a.follow[0] && !a.follow[1] || k.bind("scroll." + e, function () { if (h.x || h.y) { var d = {}; h.x && (d.left = a.follow[0] ? m(!g) : "auto"); h.y && (d.top = a.follow[1] ? p(!g) : "auto"); b.dequeue().animate(d, a.followSpeed, a.followEasing) } }).bind("resize." + e, function () { I() }); a.escClose && f.bind("keydown." + e, function (a) { 27 == a.which && r() }) } function G(d) { function c(e) { b.css({ display: "block", opacity: 1 }).animate(e, a.speed, a.easing, function () { K(d) }) } switch (d ? a.transition : a.transitionClose || a.transition) { case "slideIn": c({ left: d ? m(!(!a.follow[0] && n || g)) : f.scrollLeft() - (u || b.outerWidth(!0)) - 200 }); break; case "slideBack": c({ left: d ? m(!(!a.follow[0] && n || g)) : f.scrollLeft() + w + 200 }); break; case "slideDown": c({ top: d ? p(!(!a.follow[1] && q || g)) : f.scrollTop() - (t || b.outerHeight(!0)) - 200 }); break; case "slideUp": c({ top: d ? p(!(!a.follow[1] && q || g)) : f.scrollTop() + y + 200 }); break; default: b.stop().fadeTo(a.speed, d ? 1 : 0, function () { K(d) }) } } function K(d) { d ? (M(), l(E), a.autoClose && (H = setTimeout(r, a.autoClose))) : (b.hide(), l(a.onClose), a.loadUrl && (a.contentContainer.empty(), b.css({ height: "auto", width: "auto" }))) } function m(a) { return a ? x + f.scrollLeft() : x } function p(a) { return a ? z + f.scrollTop() : z } function l(a, e) { c.isFunction(a) && a.call(b, e) } function C() { z = q ? a.position[1] : Math.max(0, (y - b.outerHeight(!0)) / 2 - a.amsl); x = n ? a.position[0] : (w - b.outerWidth(!0)) / 2; h = D() } function D() { return { x: w > b.outerWidth(!0), y: y > b.outerHeight(!0) } } c.isFunction(A) && (E = A, A = null); var a = c.extend({}, c.fn.bPopup.defaults, A); a.scrollBar || c("html").css("overflow", "hidden"); var b = this, f = c(document), k = c(window), y = k.height(), w = k.width(), N = /OS 6(_\d)+/i.test(navigator.userAgent), v = 0, e, h, q, n, g, z, x, t, u, J, H; b.close = function () { r() }; b.reposition = function (a) { I(a) }; return b.each(function () { c(this).data("bPopup") || (l(a.onOpen), v = (k.data("bPopup") || 0) + 1, e = "__b-popup" + v + "__", q = "auto" !== a.position[1], n = "auto" !== a.position[0], g = "fixed" === a.positionStyle, t = b.outerHeight(!0), u = b.outerWidth(!0), a.loadUrl ? L() : B()) }) }; c.fn.bPopup.defaults = { amsl: 50, appending: !0, appendTo: "body", autoClose: !1, closeClass: "b-close", content: "ajax", contentContainer: !1, easing: "swing", escClose: !0, follow: [!0, !0], followEasing: "swing", followSpeed: 500, iframeAttr: 'scrolling="no" frameborder="0"', loadCallback: !1, loadData: !1, loadUrl: !1, modal: !0, modalClose: !0, modalColor: "transparent", onClose: !1, onOpen: !1, opacity: .7, position: ["auto", "auto"], positionStyle: "absolute", scrollBar: !0, speed: 250, transition: "fadeIn", transitionClose: !1, zIndex: 9997 } })(jQuery);
//}

//Finds the Querystring value for Specific Key
function GetQueryStringParams(sParam) {
    var returnString = '';
    var sPageURL = window.location.search.substring(1);
    var sURLVariables = sPageURL.split("&");
    for (var i = 0; i < sURLVariables.length; i++) {
        var sParameterName = sURLVariables[i].split("=");
        if (sParameterName[0] == sParam) {
            returnString = sParameterName[1];
        }
    }
    return returnString;
}

//Created By: Vishnu ,To Blink the Image
function blink() {
    $('.blink').delay(200).fadeTo(200, 0.5).delay(200).fadeTo(200, 1, blink);
}
// Student Banner Block starts
//Created By: Vishnu , to Load Student Banner Dyanamically
var disableNext = false;
var disablePrevious = false;
function LoadStudentBanner() {
    if ($("#studentBanner").length > 0) {
        // window.localStorage.setItem('SelectedStudentID', 1);
        var schoolID = 0;// window.localStorage.getItem('SelectedSchoolId');
        var studentID = window.localStorage.getItem('SelectedStudentID');


        if (studentID != "" && studentID > 0 && studentID != 'null' && studentID != null) {
            var htmlString = "";
            htmlString = '<div class="row"><div class="col-sm-9">';
            $.getJSON('/Common/StudentBannerFieldsWithValues?schoolID=' + schoolID + '&studentID=' + studentID,
          function (data) {

              if (data.result1.length > 0) {
                  data.result1 = JSON.stringify(data.result1)
                  var jsonObject = JSON.parse(data.result1);
                  var FirstRowString = '<div class="row">', SecondRowString = '<div class="row">';
                  //htmlString = '<div class="row"><div class="col-sm-9">';
                  for (i = 0; i < jsonObject.length; i++) {
                      var obj = jsonObject[i];
                      if (obj["LinePosition"] == "FirstRow") {
                          if (obj["ColumnName"] == "LastName" || obj["ColumnName"] == "FirstName") { //to Provide Link for Lastname/FirstName which will redirects to Student Demographics Page 
                              FirstRowString += '<div class="col-sm-3"><strong>' + obj["DisplayName"].replace('_', ' ') + '</strong> : ' + '<a href="../Students/StudentDemographics" style="cursor: pointer;" > ' + obj["ColumnValue"] + ' </a> </div> ';
                          }
                          else {
                              FirstRowString += '<div class="col-sm-3"><strong>' + obj["DisplayName"].replace('_', ' ') + '</strong> : ' + '<label> ' + obj["ColumnValue"] + '</label> </div> ';
                          }
                      }
                      else if (obj["LinePosition"] == "SecondRow") {
                          if (obj["ColumnName"] == "LastName" || obj["ColumnName"] == "FirstName") { //to Provide Link for Lastname/FirstName which will redirects to Student Demographics Page 
                              SecondRowString += '<div class="col-sm-3"><strong>' + obj["DisplayName"].replace('_', ' ') + '</strong> : ' + '<a href="../Students/StudentDemographics" style="cursor: pointer;" > ' + obj["ColumnValue"] + ' </a> </div> ';;
                          }
                          else {
                              SecondRowString += '<div class="col-sm-3"><strong>' + obj["DisplayName"].replace('_', ' ') + '</strong> : ' + '<label> ' + obj["ColumnValue"] + '</label> </div> ';;
                          }
                      }

                  }
                  htmlString += FirstRowString + '</div>';
                  htmlString += SecondRowString + '</div>';
              }
              htmlString += '</div>';

              //Auto Increment,decrement  Block Start
              htmlString += '<div class="col-sm-1 text-center"><div class="row">';
              htmlString += '<div class="input-group">';
              htmlString += '<button class="Increment btn-arrow btn-default" title="Increment" type="button"  onclick="SetNextStudent(this);return false;"><img src="../assets/img/sort-asc.png" class="btnNextStudent" /></button></br>';
              htmlString += '<button class="Decrement btn-arrow btn-default" title="Decrement" type="button" onclick="SetPreviousStudent(this);return false;"><img src="../assets/img/sort-desc.png" class="btnPreviousStudent" /></button>';
              htmlString += '</div>';
              htmlString += '</div></div>';
              //Auto Increment,decrement Block End


              htmlString += '<div class="bLeft col-sm-2"><div class="row"><div class="col-sm-12 padLeft50">';
              htmlString += '<strong>Alerts</strong></div></div><div class="row">'
              if (data.result2.length > 0) {
                  var alertsData = data.result2;
                  // console.log(alertsData);
                  if (alertsData.length > 0) {
                      alertsData = JSON.stringify(alertsData)
                      var jsonObject1 = JSON.parse(alertsData);
                      for (i = 0; i < jsonObject1.length; i++) {
                          var obj1 = jsonObject1[i];
                          for (key in obj1) {
                              var value1 = obj1[key];
                              if (value1 != null) {
                                  // console.log("Key is " + key + " Value is " + value1);
                                  if (key == "BirthdayStatus" && value1 == true) {
                                      htmlString += '<a href="#"><img class="blink alerticons" title="' + obj1['BirthDayAlertName'] + '" src="' + obj1['BirthdayImagePath'] + '"  /></a>';
                                  }
                                  else if (key == "HealthStatus" && value1 == true) {
                                      htmlString += '<a href="' + obj1['HealthPath'] + '"><img class="blink alerticons" title="' + obj1['HealthAlertName'] + '" src="' + obj1['HealthImagePath'] + '"  /></a>';
                                  }
                                  else if (key == "ParentStatus" && value1 == true) {
                                      htmlString += '<a href="' + obj1['ParentPath'] + '"><img class="blink alerticons" title="' + obj1['ParentAlertName'] + '" src="' + obj1['ParentImagePath'] + '"  /></a>';
                                  }
                                  else if (key == "DisabilityStatus" && value1 == true) {
                                      htmlString += '<a href="' + obj1['DisabilityPath'] + '"><img class="blink alerticons" title="' + obj1['DisabilityAlertName'] + '" src="' + obj1['DisabilityImagePath'] + '"  /></a>';
                                  }
                                  else if (key == "OpenEnrollmentStatus" && value1 == true) {
                                      htmlString += '<a href="' + obj1['OpenEnrollmentPath'] + '"><img class="blink alerticons" title="' + obj1['OpenEnrollmentAlertName'] + '" src="' + obj1['OpenEnrollmentImagePath'] + '"  /></a>';
                                  }
                                  else if (key == "DisciplineStatus" && value1 == true) {
                                      htmlString += '<a href="' + obj1['DisciplinePath'] + '"><img class="blink alerticons" title="' + obj1['DisciplineAlertName'] + '" src="' + obj1['DisciplineImagePath'] + '"  /></a>';
                                  }
                                  else if (key == "AccommodationsStatus" && value1 == true) {
                                      htmlString += '<a href="' + obj1['AccommodationsPath'] + '"><img class="blink alerticons" title="' + obj1['AccommodationsAlertName'] + '" src="' + obj1['AccommodationsImagePath'] + '"  /></a>';
                                  }
                                  else if (key == "LocatorStatus" && value1 == true) {
                                      //  htmlString += '<a  href="' + obj1['LocatorPath'] + '"><img class="blink alerticons" title="' + obj1['LocatorAlertName'] + '" src="' + obj1['LocatorImagePath'] + '"  /></a>';
                                      htmlString += '<a  data-toggle="tooltip" data-placement="top" title="' + obj1['LocatorAlertName'] + '" href="' + obj1['LocatorPath'] + '"><img class="blink alerticons" title="Locator" src="' + obj1['LocatorImagePath'] + '"  /></a>';
                                  }
                                  else if (key == "MiscellaneousStatus" && value1 == true) {
                                      htmlString += '<a href="' + obj1['MiscellaneousPath'] + '"><img class="blink alerticons" title="' + obj1['MiscellaneousAlertName'] + '" src="' + obj1['MiscellaneousImagePath'] + '"  /></a>';
                                  }

                              }

                          }
                      }

                  }
              }

              htmlString += '</div></div></div>';
              $("#studentBanner").html(htmlString);
              blink();
              //Disable Block
              {
                  disableNext = window.localStorage.getItem('disableNext');

                  if (disableNext == "true") {
                      $('.Increment').prop('disabled', true);//true
                  }
                  else {
                      $('.Increment').prop("disabled", false);
                  }

                  disablePrevious = window.localStorage.getItem('disablePrevious');
                  if (disablePrevious == "true") {
                      $('.Decrement').prop('disabled', true);//true
                  }
                  else {
                      $('.Decrement').prop("disabled", false);
                  }

              }

              //result1}
          });

        }
        else {
            document.getElementById("displayMessage").innerHTML = 'Please select a  student in Students Information page';
            $('#displayMessage').prepend('<span class="button b-close"><span>X</span></span>');//Prepending Close button to popup
            $('#displayMessage').bPopup({
                autoClose: 2000,
                onClose: function () { window.location = "/Students/StudentsInformation"; }
            });
        }
    }
}



//Created By: Vishnu,Sets the Previous student based on Current StudentID
function SetPreviousStudent(_this) {

    var studentsList = window.localStorage.getItem('StudentsList');
    var currentStudentID = window.localStorage.getItem('SelectedStudentID');
    var returnStudentID = SortArray(studentsList, currentStudentID, 'Previous');

    if (returnStudentID != -1 && returnStudentID != undefined) {
        //Set the Returned Student Id to Local storage and Reload the Page
        window.localStorage.setItem('SelectedStudentID', returnStudentID);
        window.location.reload();
    }
}

//Created By: Vishnu,Sets the Next student based on Current StudentID
function SetNextStudent(_this) {
    var studentsList = window.localStorage.getItem('StudentsList');
    var currentStudentID = window.localStorage.getItem('SelectedStudentID');
    var returnStudentID = SortArray(studentsList, currentStudentID, 'Next');
    if (returnStudentID != -1 && returnStudentID != undefined) {
        //Set the Returned Student Id to Local storage and Reload the Page
        window.localStorage.setItem('SelectedStudentID', returnStudentID);
        window.location.reload();
    }
}

//Created By: Vishnu
//Sorts the Array in Ascending Order and REturns Next/Previous Student If Exists
function SortArray(commaSeperatedString, currentStudentID, mode) {
    var studentsArray = commaSeperatedString.split(',');
    // alert("commaSeperatedString  " + commaSeperatedString);
    studentsArray.sort(function (a, b) { return a - b });//Sorts the Array in Ascending Order
    var currentElementIndex = -1;
    var retutnValue = -1;
    for (var i = 0; i < studentsArray.length; i++) {
        //   alert("studentsArray[i] " + studentsArray[i]);
        if (studentsArray[i] == parseInt(currentStudentID)) {
            currentElementIndex = i;
            // alert("currentElementIndex  " + currentElementIndex + "  Val is  " + studentsArray[i]);
        }
    }

    if (mode == 'Next') {
        //  if ((parseInt(currentElementIndex) == studentsArray.length-1) || (parseInt(currentElementIndex)) == 0) {
        if ((parseInt(currentElementIndex) == studentsArray.length - 1)) {
            retutnValue = -1;//No Next Student Exist Case
        }
        else {
            retutnValue = studentsArray[currentElementIndex + 1];
        }

    }
    else if (mode == 'Previous') {
        if (parseInt(currentElementIndex) == 0) {
            retutnValue = -1;//No Previos Student Exist Case
        }
        else {
            retutnValue = studentsArray[currentElementIndex - 1];
        }
    }
    //for Disable Condition
    if (studentsArray.length >= 2) {
        if ((parseInt(currentElementIndex) == studentsArray.length - 2)) {
            window.localStorage.setItem('disableNext', true);
        }
        else {
            window.localStorage.setItem('disableNext', false);
        }
    }
    if (parseInt(currentElementIndex) == 1 && mode == 'Previous') {
        window.localStorage.setItem('disablePrevious', true);
    }
    else {
        window.localStorage.setItem('disablePrevious', false);
    }


    //  alert("retutnValue  " + retutnValue);
    return retutnValue;

}

/* 
* Created by : VishnuVardhan Reddy Ammana
* Created Date: 09/Nov/2015
* For Printing Data Table
*/
function printData(tableName) {
    var divToPrint = document.getElementById(tableName);
    var printableTable = $(divToPrint).clone();
    var columnLen = printableTable.find("tr:nth-child(1)").find("th").size();
    for (var i = 1; i <= columnLen; i++) {
        var sum = 0;
        printableTable.find("tr td:nth-child(" + i + ")").each(function () {
            var nr = Number($(this).html());
            sum += nr;
        });

        printableTable.find("tr th:nth-child(" + i + ")").each(function () {
            if ($(this).hasClass('hide_column') || $(this)[0].innerHTML.toLowerCase() == 'edit' || $(this)[0].innerHTML.toLowerCase() == 'delete') {
                $(this).hide();
            }
        });

        printableTable.find("tr td:nth-child(" + i + ")").each(function () {
            if ($(this).hasClass('hide_column') || ($(this)[0].innerHTML.toLowerCase().indexOf('class="edit"') > -1)  || ($(this)[0].innerHTML.toLowerCase().indexOf('class="delete"') > -1)) {
                $(this).hide();
            }
        });
    }

    //var data = '<table border="1" cellspacing="0">' +
    var selectedSchoolName = $("#ddlSchools option:selected").text();
    var data = '<h3 style="text-align:center">EVOLVE</h3>';
    data += '<h4 style="text-align:center">School Name: ' + selectedSchoolName + '</h4>';

    //sno start
    printableTable.find("tr th:first").before("<td>SNO</td>");
    var sno = 1;
    printableTable.find("tbody").children('tr').each(function () {
        //var trow = printableTable.find("tbody").children('tr:first');
        $(this).prepend('<td>' + sno + '</td>');
        sno += 1;
    });
    //sno end

    data += '<table border="1" cellspacing="0">' +
       printableTable.html()
   + '</table>';
    //data += '<br/><button onclick="window.print()"  class="noprint">Print the Report</button>';
    data += '<style type="text/css" media="print"> .noprint {visibility: hidden;} </style>';
    myWindow = window.open('', '', 'width=800,height=600', true);
    myWindow.document.title = "Eveolve - Print";
    myWindow.innerWidth = screen.width;
    myWindow.innerHeight = screen.height;
    myWindow.screenX = 0;
    myWindow.screenY = 0;
    myWindow.document.body.innerHTML = data;
    myWindow.print();
    myWindow.close();
}



/* 
* Created by : Ram Mohan
* Created Date: 15/March/2014
*/
var specialKeys = new Array();
specialKeys.push(8); //Backspace
specialKeys.push(9); //Tab
specialKeys.push(46); //Delete
specialKeys.push(36); //Home
specialKeys.push(35); //End
specialKeys.push(37); //Left
specialKeys.push(39); //Right
specialKeys.push(32); //Space

/*
Created By: Ram Mohan
Created On: 08-April-2015
//validation for only chars
*/
function OnlyAlphabets(e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 65 && keyCode <= 90) || (keyCode <= 32) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
    return ret;
}

/*
Created By: Ram Mohan
Created On: 08-April-2015
//validation for only numbers
*/
function OnlyNumbers(e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode >= 48 && keyCode <= 57) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
    return ret;
}


/*
Created By: Ram Mohan
Created On: 08-April-2015
//validation for only numbers and .
*/
function OnlyNumbersandDot(e) {
    if (e.shiftKey || e.ctrlKey || e.altKey) {
        e.preventDefault();
    } else {
        var key = (e.which) ? e.which : e.keyCode;
       // var key = e.keyCode;
        if (!((key == 8) || (key == 46) || (key >= 48 && key <= 57))) {
            e.preventDefault();
        }
    }
}

/*
Created By: Ram Mohan
Created On: 08-April-2015
//validation for only AlphaNumeric

*/
function OnlyAlphaNumerics(e) {
    if (e.shiftKey || e.ctrlKey || e.altKey) {
        e.preventDefault();
    } else {
        var key = (e.which) ? e.which : e.keyCode;
       // var key = e.keyCode;
        if (!((key == 8) || (key == 32) || (key == 46) || (key >= 35 && key <= 40) || (key >= 65 && key <= 90) || (key >= 48 && key <= 57) || (key >= 96 && key <= 105))) {
            e.preventDefault();
        }
    }
}
/*
Created By: Ram Mohan
Created On: 08-April-2015
////validation for date
*/
function IsAlphaNumeric(e) {
    var keyCode = e.keyCode == 0 ? e.charCode : e.keyCode;
    var ret = ((keyCode <= 32) || (keyCode >= 48 && keyCode <= 57) || (keyCode >= 65 && keyCode <= 90) || (keyCode >= 97 && keyCode <= 122) || (specialKeys.indexOf(e.keyCode) != -1 && e.charCode != e.keyCode));
    return ret;
}

/*
Created By: Ram Mohan
Created On: 08-April-2015
////validation for date
*/
function isDate(txtDate) {

    var currVal = txtDate;

    if (currVal == '')
        return false;

    var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
    var dtArray = currVal.match(rxDatePattern); // is format OK?

    if (dtArray == null)
        return false;

    //Checks for mm/dd/yyyy format.
    dtMonth = dtArray[1];
    dtDay = dtArray[3];
    dtYear = dtArray[5];

    if (dtMonth < 1 || dtMonth > 12)
        return false;
    else if (dtDay < 1 || dtDay > 31)
        return false;
    else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
        return false;
    else if (dtMonth == 2) {
        var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
        if (dtDay > 29 || (dtDay == 29 && !isleap))
            return false;
    }
    return true;
}

/*
Created By: Ram Mohan
Created On: 08-April-2015
//this validation for dynamic generated textboxes having same id for both controls
*/

function CompareDates(obj) {


    var startdate = 'txtSDate-';
    var enddate = 'txtEDate-';

    var enddateid = $(obj).attr("id");

    var str = enddateid.split("-");


    var enddateseqno = str[1];


    //getting the last char of string
    // enddateseqno = enddateid.charAt(enddateid.length - 1);

    var newstartdate = startdate + enddateseqno;
    var newenddate = enddate + enddateseqno;



    var mstartdate = document.getElementById(newstartdate).value;
    var menddate = document.getElementById(newenddate).value;


    var Date1 = new Date(mstartdate);
    var Date2 = new Date(menddate);

    if (Date1 > Date2) {
        document.getElementById("displayMessage").innerHTML = 'End Date should be greater than Start Date..';
        ShowPopup();
        document.getElementById(newenddate).value = "";
        document.getElementById(newenddate).focus();
        //return false;

    }
}

/*
Created By: VishnuVardhan Reddy Ammana
Created On: 08-April-2015
To validate the form and displaying model error messae at the element if not valid
*/
function validateForm(formName) {
    //form validation block starts //Created By: Vishnu
    var _this = $(this);
    var _form = $(formName);//_this.closest("form");
    // obtain validator
    var validator = $("form").validate();

    var anyError = false;
    _form.find("input[type=text], textarea, select").each(function () {
        // validate every input element inside this step
        if (!validator.element(this)) {
            //if ($(this).hasClass("form-control input-validation-error")) {
            var classname = "error";
            var elmid = $(this).id;
            var errorMsg = $(this).attr("data-val-required");//$(this).getAttribute("data-val-required") 
            $(this).addClass("form-control input-validation-error");
            var _span = $(this).parent().find('.field-validation-valid');

            $(_span).removeClass("field-validation-valid")
            $(_span).addClass("field-validation-error");
            $(_span).html('<span class="' + classname + '" for="' + elmid + '" generated="true"> ' + errorMsg + '</span>');

            anyError = true;
        }
    });

    if (anyError) {
        return false;// exit if any error found  so that we can avoid post
    }
    else {
        return true;
    }
    //form validation block end
}