// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

if (typeof (Recorder) === "undefined") { Recorder = { __namespace: true } }

Recorder.ELEMENT_NODE = 1;

Recorder.attachDocumentEvents = function () {
    debugger;
    if (document != null) {
        if (document.addEventListener) {
            document.addEventListener('mouseup', Recorder.clickHandler, false);
            document.addEventListener('keypress', Recorder.keyHandler, false);
            document.addEventListener('change', Recorder.changeHandler, false);
        }
        else if (document.attachEvent) {
            document.attachEvent('mouseup', function (e) { return Recorder.clickHandler(e || window.event); });
            document.attachEvent('keypress', function (e) { return Recorder.keyHandler(e || window.event); });
            document.attachEvent('change', function (e) { return Recorder.changeHandler(e || window.event); });
        }
        else {
            document.onclick += Recorder.clickHandler;
            document.onkeypress += Recorder.keyHandler;
            document.onchange += Recorder.changeHandler;
        }
    }
}

Recorder.detatchDocumentEvents = function () {
    if (document != null) {
        if (document.removeEventListener) {
            document.removeEventListener('mouseup', Recorder.clickHandler);
            document.removeEventListener('keypress', Recorder.keyHandler);
            document.removeEventListener('change', Recorder.changeHandler);
        }
        else if (document.detatchEvent) {
            document.detatchEvent('mouseup');
            document.detatchEvent('keypress');
            document.detatchEvent('change');
        }
        else {
            document.onclick -= Recorder.clickHandler;
            document.onkeypress -= Recorder.keyHandler;
            document.onchange -= Recorder.changeHandler;
        }
    }
}

Recorder.clickHandler = function (event) {
    if (event == null) {
        event = window.event;
    }
    Recorder.eventHandler(event, "Click");
}

Recorder.keyHandler = function (event) {
    if (event == null) {
        event = window.event;
    }
    debugger;
    Recorder.eventHandler(event, "Keypress");
}

Recorder.changeHandler = function (event) {
    if (event == null) {
        event = window.event;
    }
    Recorder.eventHandler(event, "Change");
}

Recorder.eventHandler = function (event, eventType) {
    var target = "target" in event ? event.target : event.srcElement;
    var value = (eventType === "Click" || eventType === "Change")
        ? (target.tagName.toLowerCase() === "select") ? target.options[target.selectedIndex].value : target.value
        : null;
    var text = (eventType === "Change" && target.tagName.toLowerCase() === "select")
        ? target.options[target.selectedIndex].text
        : target.innerText;

    var data = {
        "Event": "Event : " + eventType,
        "Id": Recorder.uniqueId(),
        "CssSelector": Recorder.getCssSelectorOF(target),
        "ElementId": Recorder.getElementId(target),
        "XPathValue": Recorder.getPathTo(target),
        "KeyCode": Recorder.getKeyCode(),
        "DateTime": Recorder.getDate(),
        "IFRAME": Recorder.getFrame(),
        "Text": text,
        "Value": value
    };

    Recorder.getEvents().push(data);
}

Recorder.getEvents = function () {
    if (document.eventCollection !== undefined)
        return document.eventCollection;
    else if (parent.document.eventCollection !== undefined)
        return parent.document.eventCollection;
    else
        return null;
}

Recorder.removeEvents = function (number) {
    var events = Recorder.getEvents();

    events.splice(0, number);
}

Recorder.getDate = function () {
    var date = new Date().toJSON();
    return date.toString();
}

Recorder.getFrame = function () {
    var frame = window.frameElement;
    if (frame != null)
        return frame.id.toString();
    else
        return "";
}

Recorder.getKeyCode = function (event) {
    if (window.event.keyCode)
        return window.event.keyCode.toString();
    else if (event)
        return event.keyCode.toString();
    else
        return null;
}

Recorder.uniqueId = function () {
    var d = new Date().getTime();
    d += (parseInt(Math.random() * 100)).toString();
    return d;
}

Recorder.getElementId = function (element) {
    var selector = '';
    if (element instanceof Element && element.nodeType === Recorder.ELEMENT_NODE && element.id) {
        selector = element.id;
    }
    return selector;
}

Recorder.preventEvent = function (event) {
    if (event.preventDefault) {
        event.preventDefault();
    }
    event.returnValue = false;
    if (event.stopPropagation) {
        event.stopPropagation();
    } else {
        event.cancelBubble = true;
    }
    return false;
}

Recorder.getXY = function (element) {
    var x, y;
    x = 0;
    y = 0;
    while (element) {
        x += element.offsetLeft;
        y += element.offsetTop;
        element = element.offsetParent;
    }
    return [x, y];
}

Recorder.getPathTo = function (element) {
    var element_sibling, siblings, cnt, sibling_count;
    var elementTagName = element.tagName.toLowerCase();

    if (element.id != '') {
        return 'id("' + element.id + '")';
    } else if (element.name && document.getElementsByName(element.name).length === 1) {
        return '//' + elementTagName + '[@name="' + element.name + '"]';
    }

    if (element === document.body) {
        return '/html/' + elementTagName;
    }

    sibling_count = 0;
    siblings = element.parentNode.childNodes;
    siblings_length = siblings.length;

    for (cnt = 0; cnt < siblings_length; cnt++) {
        element_sibling = siblings[cnt];

        if (element_sibling.nodeType !== Recorder.ELEMENT_NODE) {
            continue;
        }

        if (element_sibling === element) {
            return Recorder.getPathTo(element.parentNode) + '/' + elementTagName + '[' + (sibling_count + 1) + ']';
        }

        if (element_sibling.nodeType === 1 && element_sibling.tagName.toLowerCase() === elementTagName) {
            sibling_count++;
        }
    }
}

Recorder.getCssSelectorOF = function (element) {
    if (!(element instanceof Element))
        return null;

    var path = [];

    while (element.nodeType === Recorder.ELEMENT_NODE) {
        var selector = element.nodeName.toLowerCase();

        if (element.id) {
            if (element.id.indexOf('-') > -1) {
                selector += '[id = "' + element.id + '"]';
            } else {
                selector += '#' + element.id;
            }

            path.unshift(selector);

            break;
        } else {
            var element_sibling = element;
            var sibling_cnt = 1;

            while (element_sibling = element_sibling.previousElementSibling) {
                if (element_sibling.nodeName.toLowerCase() == selector)
                    sibling_cnt++;
            }
            if (sibling_cnt != 1)
                selector += ':nth-of-type(' + sibling_cnt + ')';
        }

        path.unshift(selector);
        element = element.parentNode;
    }

    return path.join(' > ');
}

Recorder.attachDocumentEvents();