# -*- coding: utf-8 -*-

# Define your item pipelines here
#
# Don't forget to add your pipeline to the ITEM_PIPELINES setting
# See: https://doc.scrapy.org/en/latest/topics/item-pipeline.html
import MySQLdb
import MySQLdb.cursors
import datetime


class ShowstartPipeline(object):
    def process_item(self, item, spider):
        return item


class MysqlPipeline(object):
    #采用同步的机制写入mysql
    def __init__(self):
        self.conn = MySQLdb.connect('127.0.0.1', 'root', 'Abcd1234', 'showstart', charset="utf8", use_unicode=True)
        self.cursor = self.conn.cursor()

    def process_item(self, item, spider):
        insert_sql = """
            insert IGNORE into showstarts(showname, actor, price, startime,place,url,type, front_image_path, readtime)
            VALUES (%s, %s, %s, %s, %s, %s, %s,%s,%s) 
        """
        shownametemp = item["showname"].replace('\r','').replace('\n','').replace('\t','')
        actortemp = item["actor"].replace('\r','').replace('\n','').replace('\t','')
        pricetemp = item["price"]
        timetemp = item["time"].replace('\r','').replace('\n','').replace('\t','')
        placetemp = item["place"]
        urltemp = item["url"]
        startimetemp = item["StartTime"]

        try:
            typetemp = item["type"]
        except:
            typetemp = 'NoType'

        StartOrEnd = item["StartOrEnd"]
        ReadSessionTime = datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')#现在
        ReadDate = datetime.datetime.now().strftime('%Y-%m-%d')  # 现在
        front_image_path = item["front_image_path"]
        self.cursor.execute(insert_sql, (shownametemp, actortemp, pricetemp, startimetemp, placetemp, urltemp, typetemp, front_image_path, ReadSessionTime))
        self.conn.commit()

