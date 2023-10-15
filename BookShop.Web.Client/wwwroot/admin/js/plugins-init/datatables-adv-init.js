(function($) {
    "use strict"

    var table = $('#example-advance-1').DataTable();
     
    $('#example-advance-1 tbody').on('click', 'tr', function () {
        var data = table.row( this ).data();
        alert( 'You clicked on '+data[0]+'\'s row' );
    });


    var eventFired = function ( type ) {
        var n = $('#demo_info')[0];
        n.innerHTML += '<div>'+type+' event - '+new Date().getTime()+'</div>';
        n.scrollTop = n.scrollHeight;      
    }
 
    $('#example-advance-2')
        .on( 'order.dt',  function () { eventFired( 'Order' ); } )
        .on( 'search.dt', function () { eventFired( 'Search' ); } )
        .on( 'page.dt',   function () { eventFired( 'Page' ); } )
        .DataTable();


    var table = $('#table-vietnamese').DataTable( {
        createdRow: function ( row, data, index ) {
            $(row).addClass('selected')
        },
        "language": {
            "url": "https://cdn.datatables.net/plug-ins/1.13.6/i18n/vi.json"
        },
        select: true
    });

    table.on('click', 'tbody tr', function() {
    var $row = table.row(this).nodes().to$();
    var hasClass = $row.hasClass('selected');
    if (hasClass) {
        $row.removeClass('selected')
    } else {
        $row.addClass('selected')
    }
    })
    table.on('click', 'tbody tr', function() {
        var $row = table.row(this).nodes().to$();
        var hasClass = $row.hasClass('selected');
        if (hasClass) {
            $row.removeClass('selected')
        } else {
            $row.addClass('selected')
        }
        })
    
    table.rows().every(function() {
    this.nodes().to$().removeClass('selected')
    });

})(jQuery);