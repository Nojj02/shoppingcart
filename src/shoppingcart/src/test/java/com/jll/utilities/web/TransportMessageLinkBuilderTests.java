package com.jll.utilities.web;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.hateoas.Link;

public class TransportMessageLinkBuilderTests {
    public static class BuildTests {
        @Test
        public void buildsSelfRel_starts1_ends1() {
            var result = TransportMessageLinkBuilder.Build(1, 1, 1);

            var selfHref =
                result.stream()
                    .filter(x -> x.getRel() == Link.REL_SELF)
                    .findFirst()
                    .map(x -> x.getHref())
                    .orElseThrow();

            Assertions.assertEquals("/events/carts/1-1", selfHref);
        }

        @Test
        public void buildsSelfRel_starts10_ends12() {
            var result = TransportMessageLinkBuilder.Build(10, 12, 12);

            var selfHref =
                    result.stream()
                            .filter(x -> x.getRel() == Link.REL_SELF)
                            .findFirst()
                            .map(x -> x.getHref())
                            .orElseThrow();

            Assertions.assertEquals("/events/carts/10-12", selfHref);
        }

        @Test
        public void buildsNextRel_starts10_ends12() {
            var result = TransportMessageLinkBuilder.Build(10, 12, 12);

            var nextHref =
                    result.stream()
                            .filter(x -> x.getRel() == Link.REL_NEXT)
                            .findFirst()
                            .map(x -> x.getHref())
                            .orElseThrow();

            Assertions.assertEquals("/events/carts/13-15", nextHref);
        }

        @Test
        public void buildsPreviousRel_starts10_ends12() {
            var result = TransportMessageLinkBuilder.Build(10, 12, 12);

            var previousHref =
                    result.stream()
                            .filter(x -> x.getRel() == Link.REL_PREVIOUS)
                            .findFirst()
                            .map(x -> x.getHref())
                            .orElseThrow();

            Assertions.assertEquals("/events/carts/7-9", previousHref);
        }

        @Test
        public void doesNotBuildNextRel_latestEventNumberInRangeIsLessThanEndEventNumber() {
            var result = TransportMessageLinkBuilder.Build(10, 12, 11);

            var nextRel =
                    result.stream()
                            .filter(x -> x.getRel() == Link.REL_NEXT)
                            .findFirst()
                            .orElse(null);

            Assertions.assertNull(nextRel);
        }

        @Test
        public void doesNotBuildPreviousRel_starts1_ends10() {
            var result = TransportMessageLinkBuilder.Build(1, 10, 10);

            var previousRel =
                    result.stream()
                            .filter(x -> x.getRel() == Link.REL_PREVIOUS)
                            .findFirst()
                            .orElse(null);

            Assertions.assertNull(previousRel);
        }

        @Test
        public void previousRelLowestStartValueIsOne_starts2_ends10() {
            var result = TransportMessageLinkBuilder.Build(3, 10, 10);

            var previousHref =
                    result.stream()
                            .filter(x -> x.getRel() == Link.REL_PREVIOUS)
                            .findFirst()
                            .map(x -> x.getHref())
                            .orElseThrow();

            Assertions.assertEquals("/events/carts/1-2", previousHref);
        }
    }
}
