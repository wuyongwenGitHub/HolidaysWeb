/**
 * Created by zx1984 2018/3/6
 * https://github.com/zx1984
 */
'use strict';

// 模拟数据
function createMockData () {
  var mockData = []
  for (var i = 0; i < 100; i++) {
    mockData.push({
      date: '2018-'+ fd(i%12 + 1) +'-' + fd(randNum(30)),
      stock: i*21,
      buyNumMax: "50",
      buyNumMin: "1",
      price: randNum(i) + '.00',
      priceMarket: "100.00",
      priceSettlement: "90.00",
      priceRetail: "99.00"
    });
  }
  return mockData
}

function randNum (max) {
  return Math.round(Math.random() * max);
}

function fd (n) {
  n = n.toString();
  return n[1] ? n : '0' + n;
}
