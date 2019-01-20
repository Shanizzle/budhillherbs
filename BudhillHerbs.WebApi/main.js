
$(function () {

    getDailyTask();
    getTinctures();

    $('.dropdown-foxhole-img').hover(
        function () {
            $('.dropdown-foxhole-content').addClass('hover');
        },
        function () {
            $('.dropdown-foxhole-content').removeClass('hover');
        }
    );
});

function getDailyTask() {
    
    $.ajax({
        type: 'GET',
          url: 'http://kortego-vm2:50421/api/task/getdailys',
       // url: 'http://test.budhillherbs.com/api/task/getdailys',

        success: function (response) {
            buildDailyTaskData(response);
        },
        error: function (jqXHR, exception) {
            //alert(JSON.stringify(jqXHR)  + ' ' + exception);
        }
    });
}


function buildDailyTaskData(response) {
	
	var rows = '';

	for (var i in response) {
		// build out each row
		var row = '<tr>';

        var td = '<td><div class="completeDiv">' + response[i].PreparationName + '</div></td>';
		row += td;

        td = '<td>' + response[i].Name + '</td>';
		row += td;

        var date = new Date(Date.parse(response[i].ReadyDate)); 
        date = moment(date).format("MM/DD/YY");

        td = '<td>' + date + '</td>';
        row += td;

        // here is the last thing we did, basically updating the html template
        // 1. set the class so we can assign a jquery click event to each button
        // 2. added new attribute data-id to the <button> element to keep track of each ID from database, grab it from response in for loop
        // 3. scroll down

        //<i class="fas fa-check"></i>
        if (response[i].Status) {

            td = '<td class="completeTd"><i class="fas fa-check"></i></td>';
        }
        else {

            td = '<td class="completeTd"><div class="completeDiv"><button class="complete-button" type="button" data-id="' + response[i].ID + '">Complete</button></div></td>';

        }
        
		row += td;
		row += '</tr>';

		// add row to rows
		rows += row;
	}
    
	// now add the html rows to the dom

    $('#dailyTasks tr:last').after(rows);

    // OH.. btw, the jquery selecter will return an array of ui elements, in this case a bunch of buttons YAY
    // ok so the .on("click") the second parameter is the callback function.. AKA the closure, its called once the button is clicked (bye for now)

    $('.complete-button').on("click", function () {
        console.log("button clicked");

        // 3. continue... retreive the data-id attribute from this (the button that was clicked)
        // 4. plug in the id to the ajax url, remember to concatenate the id to the end of url
        // 5. the ajax call then calls into the api method to do the actual update, 
        //    it returns the data which is a bool and is set in the response parameter in success

        // the var id in this javascript method is in no way related to the C# web method parameter int id 

        var id = $(this).attr("data-id");
        var btn = $(this);

        $.ajax({
            type: 'POST',
            //url: 'http://kortego-vm2:50421/api/task/complete/' + id,
            url: 'http://test.budhillherbs.com/api/task/complete/' + id,

            success: function (response) {
                if (response) {
                    btn.hide();
                    //btn.css({ visibility: 'hidden' });
                    btn.parent().append('<i class="fas fa-check gridCheckMark"></i>');

                }
            },
            error: function (jqXHR, exception) {
                //alert(JSON.stringify(jqXHR)  + ' ' + exception);
            }
        });

    });
}



function getTinctures() {

    $.ajax({
        type: 'GET',
        url: 'http://kortego-vm2:50421/api/list/gettinctures',
       // url: 'http://test.budhillherbs.com/api/list/gettinctures',

        success: function (response) {
            buildTincturesListData(response);
        },
        error: function (jqXHR, exception) {
            alert(JSON.stringify(jqXHR) + ' ' + exception);
        }
    });
}

function buildTincturesListData(response) {

    var rows = '';

    for (var i in response) {
        // build out each row
        var row = '<tr>';

        //var td = '<td><div class="completeDiv">' + response[i].PreparationName + '</div></td>';
        //row += td;

        td = '<td>' + response[i].Name + '</td>';
        row += td;

        td = '<td>' + response[i].LotID + '</td>';
        row += td;

        var date = new Date(Date.parse(response[i].DateIn));
        date = moment(date).format("MM/DD/YY");

        td = '<td>' + date + '</td>';
        row += td;

        td = '<td>' + response[i].TotalOz + 'oz' + '</td>';
        row += td;

        // here is the last thing we did, basically updating the html template
        // 1. set the class so we can assign a jquery click event to each button
        // 2. added new attribute data-id to the <button> element to keep track of each ID from database, grab it from response in for loop
        // 3. scroll down

        //<i class="fas fa-check"></i>
        //if (response[i].Status) {

        //    td = '<td class="completeTd"><i class="fas fa-check"></i></td>';
        //}
        //else {

        //    td = '<td class="completeTd"><div class="completeDiv"><button class="complete-button" type="button" data-id="' + response[i].ID + '">Complete</button></div></td>';

        //}

        
        row += '</tr>';

        // add row to rows
        rows += row;
    }

    // now add the html rows to the dom

    $('#tincturesList tr:last').after(rows);


}