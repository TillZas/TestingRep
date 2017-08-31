reloadBooks(null, null);

$.getJSON("/Library/BookTitles", function (adata) {
    $("#searchLine").kendoAutoComplete({
        animation: false,
        dataSource: adata,
        change: function (e) {
            updateGrid();
        }
    });
});


$.getJSON("/Library/AuthorsEnum", function (adata) {
    adata[0].text = "Любой автор";
    $("#authorSearch").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: adata,
        change: function (e) {
            updateGrid();
        }
    });
});

function updateGrid() {
    var title = $("#searchLine").data("kendoAutoComplete").value();
    var authorRefId = $("#authorSearch").data("kendoDropDownList").value();
    var grid = $("#libraryBookStack").data("kendoGrid");
    grid.setDataSource(getDataSource(authorRefId, title));
}

function getDataSource(author, title) {
    var data = new kendo.data.DataSource({
        transport: {
            read: {
                url: "/Library/Books",
                dataType: "json",
                data: { authorId: author, title: title }
            }

        }, pageSize: 10
    });
    return data;
}

function removeBook(e) {
    //console.log("Removing ", e.model.bookId);
    $.ajax({
        url: "/Library/RemoveBook?id=" + e.model.bookId,
        context: document.body
    });/*.done(function () {
        console.log("Removed ", e.model.bookId);
        //reloadBooks(null, null);
    });*/
}

function reloadBooks(author, title) {

    var data = getDataSource(author, title);

    var authorData = null;

    $.getJSON("/Library/AuthorsEnum", function (adata) {
        //alert(JSON.stringify(data));  
        $("#libraryBookStack").kendoGrid({
            editable: {
                mode: "incell"
            },
            pageable: {
                pageSizes: [2, 4, 8, 16, 32],
                refresh: true
            },
            selectable: "row",
            sortable: true,
            columns: [{
                field: "title",
                title: "Название"
            },
            {
                field: "annotation",
                title: "Описание"
            },
            {
                field: "authorRefId",
                title: "Имя автора",
                values: adata
            },

            { command: ["destroy"] }],
            dataSource: data,
            messages: {
                commands: {
                    create: "Добавить",
                    update: "Обновить",
                    destroy: "Удалить",
                    cancel: "Отменить"
                }
            },
            cellClose: function (e) {
                console.log(e);
                $.ajax({
                    url: "/Library/EditBook?id=" + e.model.bookId + "&title=" + e.model.title + "&annotation=" + e.model.annotation + "&authorRefId=" + (e.model.authorRefId == null ?"null": e.model.authorRefId.value),
                    context: document.body
                });
            },
            remove: removeBook
        });
    });

}


$("#button5").kendoButton({
    click: function (e) {
        $.ajax({
            url: "/Library/Add?title=НоваяКнига",
            context: document.body
        }).done(function () {
            $("#searchLine").data("kendoAutoComplete").value(null);
            $("#authorSearch").data("kendoDropDownList").value(null);
            updateGrid();
        });
    }
});

$("#button6").kendoButton({
    click: function (e) {
        $.ajax({
            url: "/Library/Add",
            context: document.body
        }).done(function () {
            $("#searchLine").data("kendoAutoComplete").value(null);
            $("#authorSearch").data("kendoDropDownList").value(null);
            updateGrid();
        });
    }
});

$("#button7").kendoButton({
    click: function (e) {

        var ddownlst = $("#authorSearchEdit").data("kendoDropDownList");

        ddownlst.select(0);
        ddownlst.trigger("change");

        var dialog = $("#authorEditor").data("kendoWindow");
        dialog.open();
    }
});

//-----------------------------------------
//Autor editing widget
//-----------------------------------------


$("#authorEditor").kendoWindow({
    visible: false
});

var authorValidator = $("#anEditor").kendoValidator({
    messages: {
        required: "Поле не может быть пустым",
    }
}).data("kendoValidator");


authorValidator.bind("validateInput", function (e) {
    $("#button8").data("kendoButton").enable(e.valid);
});

$.getJSON("/Library/AuthorsEnum", function (adata) {
    adata[0].text = "Новый автор";
    $("#authorSearchEdit").kendoDropDownList({
        dataTextField: "text",
        dataValueField: "value",
        dataSource: adata,
        change: function (e) {
            var ddownlst = $("#authorSearchEdit").data("kendoDropDownList");
            if (ddownlst.dataItem().value == null)
                $("#authorName").val("Имя нового автора");
            else
                $("#authorName").val(ddownlst.text());
        }
    });
});


$("#button8").kendoButton({
    click: function (e) {
        var author = $("#authorSearchEdit").data("kendoDropDownList").value();
        var name = $("#authorName").val();

        console.log(name);

        if (!authorValidator.validate()) return;


        $.ajax({
            url: "/Library/UpdateAuthor",
            context: document.body,
            data: { id: author, name: name }
        }).done(function () {
            $("#searchLine").data("kendoAutoComplete").value(null);
            $("#authorSearch").data("kendoDropDownList").value(null);
            updateAllAuthors();
        });
        var dialog = $("#authorEditor").data("kendoWindow");
        dialog.close();
        $("#authorName").val(null);
    }
});

function updateAllAuthors() {
    $.getJSON("/Library/AuthorsEnum", function (adata) {
        adata[0].text = "Любой автор";
        $("#authorSearch").data("kendoDropDownList").setDataSource(adata);

        adata[0].text = "Новый автор";
        $("#authorSearchEdit").data("kendoDropDownList").setDataSource(adata);


        adata[0].text = "Без автора";
        var grid = $("#libraryBookStack").data("kendoGrid");
        grid.setOptions({
            columns: [{
                field: "title",
                title: "Название"
            },
            {
                field: "annotation",
                title: "Описание"
            },
            {
                field: "authorRefId",
                title: "Имя автора",
                values: adata
            },

            { command: ["destroy"] }],
        });

        updateGrid();
    });
}


