from abc import *

class Order(metaclass=ABCMeta):
    _m_price = 0

    def GetPrice(self):
        return self._m_price

    @abstractmethod
    def DisplayOrder(self):
        pass

class Pizza(Order):
    m_pizza_type = ""
    m_topping = ""

    def __init__(self, type, topping, price):
        self.m_pizza_type = type
        self.m_topping = topping
        self._m_price = price

    def DisplayOrder(self):
        print('%s 피자 + %s 추가' % (self.m_pizza_type, self.m_topping))
        #print self.m_pizza_type + " 피자 + " + self.m_topping + " 추가"
        print("가격 : " + str(self._m_price) + "원")

        return self._m_price

class Chicken(Order):
    m_n_number = 0
    m_additional = ""

    def __init__(self, number, additional, price):
        self.m_n_number = number
        self.m_additional = additional
        self._m_price = price

    def DisplayOrder(self):
        print(str(self.m_n_number) + "마리 치킨 + " + self.m_additional + " 추가")
        print("가격 : " + str(self._m_price) + "원")

        return self._m_price

class Decorator(Order):
    def __init__(self, order):
        self.m_order = order

    def DisplayOrder(self):
        return self.m_order.DisplayOrder()

class Discountable(Decorator):
    m_discount = []
    m_n_dc_value = 0

    def __init__(self, order):
        super().__init__(order)
        self.m_n_dc_value = 0

    def AddDiscount(self, dc_desc, price):
        self.m_discount.append(dc_desc)
        self.m_n_dc_value += price

    def DisplayOrder(self):
        total = super().DisplayOrder()

        for i in self.m_discount:
            print("추가할인 : " + i)
            
        print("총 할인 금액 : " + str(self.m_n_dc_value) + "원")

        return total - self.m_n_dc_value

class AddDeliveryFee(Decorator):
    m_n_distance = 0

    def __init__(self, order):
        super().__init__(order)
        self.m_n_distance = 0

    def SetDistance(self, distance):
        self.m_n_distance = distance

    def DisplayOrder(self):
        total = super().DisplayOrder()

        add = 0
        if self.m_n_distance > 10:
            print("배달비 2000원 추가")
            add += 2000
        elif self.m_n_distance > 5:
            print("배달비 1000원 추가")
            add += 1000

        return total + add

# 피자 주문
pizza_order = Pizza("포테이토", "베이컨, 치즈크러스트", 18000)
discount = Discountable(pizza_order)
discount.AddDiscount("멤버쉽 할인 3000원", 3000)
discount.AddDiscount("첫 주문 할인 1000원", 1000)
delivery = AddDeliveryFee(discount)
delivery.SetDistance(15)

print("\n----- 현재 주문 내역 -----")
price = delivery.DisplayOrder()
print("총 주문 금액 : " + str(price) + "원")

# 치킨 주문
chicken_order = Chicken(2, "웨지감자, 파", 23000)
discount = Discountable(chicken_order)
discount.AddDiscount("5월 할인 7000원", 7000)
delivery = AddDeliveryFee(discount)
delivery.SetDistance(6)

print("\n----- 현재 주문 내역 -----")
price = delivery.DisplayOrder()
print("총 주문 금액 : " + str(price) + "원")
