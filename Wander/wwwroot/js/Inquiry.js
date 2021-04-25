

var dataTable;


$(document).ready(function () {
    loadDataTable("GetInquiryList")
});

function loadDataTable(url) {
    databale = $('#tblData').DataTable({
        "ajax": {
            "url": "/RentalInquiry/" + url
        },

        "columns": [

            { "data": "id", "width": "10%" },
            { "data": "name", "width": "15%" },
            { "data": "phonenumber ", "width": "15%" },
            { "data": "email", "width": "15%" },

            {
                "data": "id",
                "render": function (data) {
                    return `
                           <div class="text-center">
                            <a href="/RentalInquiry/Details/${data}" class="btn btn-success text-white" style="cursor:pointer">
                        <i class="fas fa-edit"></li>
                                </a>
                            </div>`;
                },
                "width": "5%"
            }

        ]
    }
    );
}