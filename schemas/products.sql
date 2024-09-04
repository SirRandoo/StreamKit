/*
    The various types of products supported by the mod.
*/
CREATE TYPE ProductType AS ENUM ( 'item', 'trait', 'pawn', 'childhood', 'adulthood', 'event' );

/*
    The table schema for products.

    This table represents the common properties all products share, regardless
    of how users may have customized said products.
 */
CREATE TABLE Products
(
    /*
        The package id of the extension or mod that the product originated
        from.
     */
    content_source_id TEXT REFERENCES ContentSources (content_source_id),

    /*
        The unique id of the product.

        NOTE: This id should be unique among its product type. If a product is
                an event, the product's id should not be the same as any other
                event products, but other product types, like an item, may
                have the same id as the product.
     */
    product_id        TEXT,

    /*
        The type of product being represented.
     */
    product_type      ProductType,

    /*
        The category of the product.

        A product may be a part of a category, but it may also be uncategorized,
        as in the case of pawns, traits, or adulthood and childhood products.
     */
    product_category  TEXT NULL,

    PRIMARY KEY (product_id, product_type)
);
