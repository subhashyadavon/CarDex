// src/__tests__/logic/TradingEngine.test.ts

import { TradingEngine } from '../../logic/TradingEngine';
import { User, Card, OpenTrade, TradeEnum, GradeEnum } from '../../types/types';

describe('TradingEngine', () => {
  let engine: TradingEngine;
  let mockSellerUser: User;
  let mockBuyerUser: User;
  let mockSellerCard: Card;
  let mockBuyerCard: Card;

  beforeEach(() => {
    engine = new TradingEngine();

    mockSellerUser = {
      id: 1,
      username: 'seller',
      password: 'hashed',
      currency: 2000,
      owned_cards: [1, 3],
      owned_packs: [],
      open_trades: [],
      trade_history: []
    };

    mockBuyerUser = {
      id: 2,
      username: 'buyer',
      password: 'hashed',
      currency: 1500,
      owned_cards: [2],
      owned_packs: [],
      open_trades: [],
      trade_history: []
    };

    mockSellerCard = {
      id: 1,
      user_id: 1,
      vehicle_id: 1,
      collection_id: 1,
      grade: GradeEnum.LIMITED_RUN,
      value: 1200
    };

    mockBuyerCard = {
      id: 2,
      user_id: 2,
      vehicle_id: 2,
      collection_id: 1,
      grade: GradeEnum.FACTORY,
      value: 1000
    };
  });

  describe('validateTrade - Currency Trades', () => {
    test('validates successful currency trade', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_PRICE,
        user_id: 1,
        card_id: 1,
        price: 1000,
        want_card_id: null
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard
      );

      expect(result.valid).toBe(true);
      expect(result.error).toBeUndefined();
    });

    test('rejects trade when buyer has insufficient funds', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_PRICE,
        user_id: 1,
        card_id: 1,
        price: 2000,
        want_card_id: null
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard
      );

      expect(result.valid).toBe(false);
      expect(result.error).toBe('Insufficient funds');
    });

    test('rejects trade with negative price', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_PRICE,
        user_id: 1,
        card_id: 1,
        price: -100,
        want_card_id: null
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard
      );

      expect(result.valid).toBe(false);
      expect(result.error).toBe('Price cannot be negative');
    });
  });

  describe('validateTrade - Card-for-Card Trades', () => {
    test('validates successful card trade', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_CARD,
        user_id: 1,
        card_id: 1,
        price: 0,
        want_card_id: 2
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard,
        mockBuyerCard
      );

      expect(result.valid).toBe(true);
      expect(result.error).toBeUndefined();
    });

    test('rejects card trade without specified wanted card', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_CARD,
        user_id: 1,
        card_id: 1,
        price: 0,
        want_card_id: null
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard
      );

      expect(result.valid).toBe(false);
      expect(result.error).toBe('Card trade must specify wanted card');
    });

    test('rejects trade when buyer does not own requested card', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_CARD,
        user_id: 1,
        card_id: 1,
        price: 0,
        want_card_id: 20
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard
      );

      expect(result.valid).toBe(false);
      expect(result.error).toBe('Buyer does not own the requested card');
    });

    test('rejects trade when buyer card does not belong to buyer', () => {
      const wrongCard: Card = {
        ...mockBuyerCard,
        user_id: 5
      };

      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_CARD,
        user_id: 1,
        card_id: 1,
        price: 0,
        want_card_id: 2
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard,
        wrongCard
      );

      expect(result.valid).toBe(false);
      expect(result.error).toBe('Requested card does not belong to buyer');
    });
  });

  describe('validateTrade - Ownership Validation', () => {
    test('rejects trade when seller does not own the card', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_PRICE,
        user_id: 1,
        card_id: 40,
        price: 1000,
        want_card_id: null
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard
      );

      expect(result.valid).toBe(false);
      expect(result.error).toBe('Seller does not own the card');
    });

    test('rejects trade when card does not belong to seller', () => {
      const wrongCard: Card = {
        ...mockSellerCard,
        user_id: 5
      };

      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_PRICE,
        user_id: 1,
        card_id: 1,
        price: 1000,
        want_card_id: null
      };

      const result = engine.validateTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        wrongCard
      );

      expect(result.valid).toBe(false);
      expect(result.error).toBe('Card does not belong to seller');
    });
  });

  describe('executeTrade', () => {
    test('executes valid trade and returns completed trade object', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_PRICE,
        user_id: 1,
        card_id: 1,
        price: 1000,
        want_card_id: null
      };

      const completed = engine.executeTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard
      );

      expect(completed.seller_user_id).toBe(1);
      expect(completed.buyer_user_id).toBe(2);
      expect(completed.seller_card_id).toBe(1);
      expect(completed.buyer_card_id).toBeNull();
      expect(completed.price).toBe(1000);
      expect(completed.type).toBe(TradeEnum.FOR_PRICE);
      expect(completed.executed_date).toBeInstanceOf(Date);
      expect(completed.id).toBeDefined();
    });

    test('executes valid card trade with buyer card', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_CARD,
        user_id: 1,
        card_id: 1,
        price: 0,
        want_card_id: 2
      };

      const completed = engine.executeTrade(
        trade,
        mockSellerUser,
        mockBuyerUser,
        mockSellerCard,
        mockBuyerCard
      );

      expect(completed.buyer_card_id).toBe(2);
      expect(completed.type).toBe(TradeEnum.FOR_CARD);
    });

    test('throws error when executing invalid trade', () => {
      const trade: OpenTrade = {
        id: 1,
        type: TradeEnum.FOR_PRICE,
        user_id: 1,
        card_id: 1,
        price: 5000,
        want_card_id: null
      };

      expect(() => {
        engine.executeTrade(
          trade,
          mockSellerUser,
          mockBuyerUser,
          mockSellerCard
        );
      }).toThrow('Insufficient funds');
    });
  });

  describe('calculateTradeFairness', () => {
    test('returns fair for cards with similar values', () => {
      const result = engine.calculateTradeFairness(1000, 1100);
      expect(result).toBe('fair');
    });

    test('returns fair when difference is exactly at threshold', () => {
      const result = engine.calculateTradeFairness(1000, 1200, 20);
      expect(result).toBe('fair');
    });

    test('returns unfair for cards with large value difference', () => {
      const result = engine.calculateTradeFairness(1000, 2000);
      expect(result).toBe('unfair');
    });

    test('returns fair for identical values', () => {
      const result = engine.calculateTradeFairness(1500, 1500);
      expect(result).toBe('fair');
    });

    test('accepts custom threshold percentage', () => {
      const result = engine.calculateTradeFairness(1000, 1500, 50);
      expect(result).toBe('fair');
    });

    test('works correctly regardless of which card is more valuable', () => {
      const result1 = engine.calculateTradeFairness(2000, 1000);
      const result2 = engine.calculateTradeFairness(1000, 2000);
      expect(result1).toBe(result2);
    });
  });
});