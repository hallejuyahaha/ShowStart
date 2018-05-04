# -*- coding: utf-8 -*-
import scrapy
from ShowStart.items import ShowStartItem
from ShowStart.items import ShowStartItemLoader
from scrapy.http import Request
from urllib import parse
from scrapy.loader import ItemLoader



class ShowstartspiderSpider(scrapy.Spider):
    name = 'ShowStartSpider'
    allowed_domains = ['www.showstart.com']
    start_urls = ['https://www.showstart.com/event/list?cityId=25&isList=1&pageNo=1']

    def parse(self, response):
        # 解析列表页中的所有文章url并交给scrapy下载后并进行解析
        post_nodes = response.css(".g-list-wrap.justify.MT30 li")[:20]
        for post_node in post_nodes:
            post_url = post_node.css("a::attr(href)").extract_first("")
            yield Request(url=parse.urljoin(response.url, post_url), callback=self.parse_detail)

        # # 提取下一页并交给scrapy进行下载
        # next_url = response.xpath("//div[@class='page']/a[@class='page-next']")
        # if next_url:
        #     yield Request(url=self.parse.urljoin(response.url, next_url), callback=self.parse)
        # pass


    def parse_detail(self, response):
        # 通过item loader加载item
        # front_image_url = response.meta.get("front_image_url", "")  # 文章封面图
        item_loader = ShowStartItemLoader(item=ShowStartItem(), response=response)
        item_loader.add_xpath("showname","//div[@class='items ll']/h1/text()[last()]")
        item_loader.add_xpath("time", "//ul[@class='items-list']/li[1]/text()")
        item_loader.add_xpath("actor", "//ul[@class='items-list']/li[2]/a/text()")
        item_loader.add_xpath("place", "//ul[@class='items-list']/li[3]/a/text()")

        item_loader.add_value("price", 1)
        item_loader.add_value("url", response.url)
        item_loader.add_css("type", ".goods-type span::text")

        Items = item_loader.load_item()
        yield Items
        pass
