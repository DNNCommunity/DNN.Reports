// Static methods to support the Reports Module client-side features

dnn_reports_showHideByCheckbox = function(checkbox, target, showWhenChecked) {
    var chkObj = dnn.dom.getById(checkbox);
    var targetObj = dnn.dom.getById(target);
    if((chkObj != null) && (targetObj != null)) {
        if(chkObj.checked == showWhenChecked)
            targetObj.style.display = '';
        else
            targetObj.style.display = 'none';
    }
}

dnn_reports_showHideByRadioButtons = function(radioA, radioB, objA, objB) {
    var rA = dnn.dom.getById(radioA)
    var rB = dnn.dom.getById(radioB)
    if((rA == null) || (rB == null))
        return;
    if(rA.checked) {
        dnn_reports_hide(objB);
        dnn_reports_show(objA);
    } else if(rB.checked) {
        dnn_reports_hide(objA);
        dnn_reports_show(objB);
    }
}

dnn_reports_updateBarColorMode = function(radioOneColor, radioColorPerBar, rowOneColor, rowColorPerBar) {
    var r1 = dnn.dom.getById(radioOneColor)
    var r2 = dnn.dom.getById(radioColorPerBar)
    if((r1 == null) || (rB == null))
        return;
    if(r1.checked) {
        dnn_reports_hide(rowColorPerBar);
        dnn_reports_show(rowOneColor);
    } else if(r2.checked) {
        dnn_reports_hide(rowOneColor);
        dnn_reports_show(rowColorPerBar);
    }
}

dnn_reports_hide = function(hideId) {
    var hideObj = dnn.dom.getById(hideId);
    hideObj.style.display = 'none';
}

dnn_reports_show = function(showId) {
    var showObj = dnn.dom.getById(showId);
    if(showObj != null)
        showObj.style.display = '';
}

dnn_reports_updateRSMode = function(localRadio, serverRadio, localFileRow, localDSRow, serverUrlRow, serverPathRow) {
    var rL = dnn.dom.getById(localRadio);
    var rS = dnn.dom.getById(serverRadio)
    if((rL == null) || (rS == null))
        return;
    if(rL.checked) {
        dnn_reports_show(localFileRow);
        dnn_reports_show(localDSRow);
        dnn_reports_hide(serverUrlRow);
        dnn_reports_hide(serverPathRow);
    } else if(rS.checked) {
        dnn_reports_hide(localFileRow);
        dnn_reports_hide(localDSRow);
        dnn_reports_show(serverUrlRow);
        dnn_reports_show(serverPathRow);
    }
}

dnn_reports_updateColorPreview = function(colorText, colorPreview) {
    var colorTextObj = document.getElementById(colorText);
    var colorPreviewObj = document.getElementById(colorPreview);
    var hcv = colorTextObj.value

    if (hcv.length > 0 && hcv.charAt(0) != '#') {
        hcv = '#' + hcv;
    }
    if (hcv.length != 4 && hcv.length != 7) {
        hcv = '#000';
    }
    var i;
    for (i = 1; i < hcv.length; i++) {
        if ('0123456789abcdefABCDEF'.indexOf(hcv.charAt(i)) == -1) {
            hcv = '#000';
            break;
        }
    }

    colorPreviewObj.style.background = hcv;
}