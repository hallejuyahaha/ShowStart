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
        self.conn = MySQLdb.connect('127.0.0.1', 'root', 'Abcd1234', 'test', charset="utf8", use_unicode=True)
        self.cursor = self.conn.cursor()

    def process_item(self, item, spider):
        insert_sql = """
            insert into showstart(showname, actor, price, time,place,url,type,StartOrEnd,ReadSessionTime,ReadDate,front_image_path)
            VALUES (%s, %s, %s, %s, %s, %s, %s,%s,%s,%s,%s) 
        """
        shownametemp = item["showname"].replace('\r','').replace('\n','').replace('\t','')
        actortemp = item["actor"].replace('\r','').replace('\n','').replace('\t','')
        pricetemp = item["price"]
        timetemp = item["time"]
        placetemp = item["place"]
        urltemp = item["url"]
        try:
            typetemp = item["type"]
        except:
            typetemp = 'NoType'

        StartOrEnd = item["StartOrEnd"]
        ReadSessionTime = datetime.datetime.now().strftime('%Y-%m-%d %H:%M:%S')#现在
        ReadDate = datetime.datetime.now().strftime('%Y-%m-%d')  # 现在
        front_image_path = item["front_image_path"]
        self.cursor.execute(insert_sql, (shownametemp, actortemp, pricetemp,  timetemp,  placetemp,  urltemp,  typetemp,StartOrEnd,ReadSessionTime,ReadDate,front_image_path))
        self.conn.commit()