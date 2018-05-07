# -*- coding: utf-8 -*-
import scrapy
from ShowStart.items import ShowStartItem
from ShowStart.items import ShowStartItemLoader
from scrapy.http import Request
from urllib import parse
from scrapy.loader import ItemLoader
from urllib.parse import urljoin
import re
import datetime
import time


class ShowstartspiderSpider(scrapy.Spider):
    name = 'ShowStartSpider'
    allowed_domains = ['www.showstart.com']
    start_urls = ['https://www.showstart.com/event/list?cityId=25&isList=1&pageNo=1']

    def parse(self, response):
        # 解析列表页中的所有文章url并交给scrapy下载后并进行解析
        post_nodes = response.css(".g-list-wrap.justify.MT30 li")[:20]
        for post_node in post_nodes:
            post_url = post_node.css("a::attr(href)").extract_first("")

            # 获取 未开始 或 已结束
            overtemp = post_node.css(".g-time")
            if (overtemp.css(".col-theme").extract_first("") == ''):
                overornot = "1"
            else:
                overornot = "0"
            # 获取 价格
            if(post_node.css(".g-price").extract_first("") == ''):
                price = 0
            else:
                pricetemp = post_node.css(".g-price .col-theme::text").extract_first("")
                match_re = re.match("￥(\d+)[\s\S]*", pricetemp)
                if match_re:
                    price = match_re.group(1)
            #传送运行
            yield Request(url=parse.urljoin(response.url, post_url),meta={ "OverOrNot": overornot, "Price":price}, callback=self.parse_detail)#"price":price,

        # 提取下一页并交给scrapy进行下载
        next_url = response.css(".page-next::attr(href)").extract_first("")
        if next_url:
            uurl =parse.urljoin(response.url, next_url)
            yield Request(url=uurl, callback=self.parse)
        pass


    def parse_detail(self, response):
        # 通过item loader加载item
        # front_image_url = response.meta.get("front_image_url", "")
        item_loader = ShowStartItemLoader(item=ShowStartItem(), response=response)
        item_loader.add_xpath("showname","//div[@class='items ll']/h1/text()[last()]")
        item_loader.add_xpath("time", "//ul[@class='items-list']/li[1]/text()")
        item_loader.add_xpath("actor", "//ul[@class='items-list']/li[2]/a/text()")
        item_loader.add_xpath("place", "//ul[@class='items-list']/li[3]/a/text()")

        item_loader.add_value("price", response.meta.get("Price", "") )
        item_loader.add_value("url", response.url)
        try:
            item_loader.add_css("type", ".goods-type span::text")
        except:
            item_loader.add_value("type", "NoType")

        item_loader.add_value("StartOrEnd", response.meta.get("OverOrNot", ""))

        Items = item_loader.load_item()
        yield Items
        pass
