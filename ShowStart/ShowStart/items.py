# -*- coding: utf-8 -*-

# Define here the models for your scraped items
#
# See documentation in:
# https://doc.scrapy.org/en/latest/topics/items.html

import scrapy
from scrapy.loader import ItemLoader
from scrapy.loader.processors import MapCompose, TakeFirst, Join



class ShowStartItem(scrapy.Item):
    showname = scrapy.Field()
    actor = scrapy.Field()
    price = scrapy.Field()
    time = scrapy.Field()
    place = scrapy.Field()
    url = scrapy.Field()
    type = scrapy.Field()

class ShowStartItemLoader(ItemLoader):
    #自定义itemloader
    default_output_processor = TakeFirst()