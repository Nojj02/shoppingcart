package com.jll.utilities.web;

import com.jll.controllers.CartEventsController;
import org.springframework.hateoas.Link;
import org.springframework.hateoas.mvc.ControllerLinkBuilder;

import java.util.ArrayList;
import java.util.List;

public class TransportMessageLinkBuilder {
    public static List<Link> Build(long startEventNumber, long endEventNumber, long latestAvailableEventNumberInRange) {
        if (latestAvailableEventNumberInRange < startEventNumber || latestAvailableEventNumberInRange > endEventNumber) {
            throw new RuntimeException("latestAvailableEventNumberInRange is out of range of start and end event numbers");
        }

        var rangeSize = endEventNumber - startEventNumber;
        var links = new ArrayList<Link>();
        links.add(ControllerLinkBuilder
                .linkTo(
                        ControllerLinkBuilder.methodOn(CartEventsController.class)
                                .get(startEventNumber, endEventNumber))
                .withSelfRel());

        if (latestAvailableEventNumberInRange == endEventNumber) {
            var nextStartEventNumber = endEventNumber + 1;
            links.add(ControllerLinkBuilder
                    .linkTo(
                            ControllerLinkBuilder.methodOn(CartEventsController.class)
                                    .get(nextStartEventNumber, nextStartEventNumber + rangeSize))
                    .withRel(Link.REL_NEXT));
        }

        if (startEventNumber > 1) {
            var previousEndEventNumber = startEventNumber - 1;
            links.add(ControllerLinkBuilder
                    .linkTo(
                            ControllerLinkBuilder.methodOn(CartEventsController.class)
                                    .get(Math.max(1, previousEndEventNumber - rangeSize), previousEndEventNumber))
                    .withRel(Link.REL_PREVIOUS));
        }
        return links;
    }
}
