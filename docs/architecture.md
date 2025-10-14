<p align="center">
  <img src="../assets/arch_header.png" alt="Arch Logo" width="800"/>
</p>

# Overview

Below is the full map of CarDex's architecture, including the Data Objects and their relationships.  
We chose to use an ID-based system to connect tables, layers, and (eventually) logic together.

> **NOTE**  
> ClaudeAI was used to assist this markdown file. The Architecture diagram and actual data flow was wholly original.

<p align="center">
  <img src="../assets/arch_full.png" alt="Full Arch" width="800"/>
</p>

<p align="center">
  <img src="../assets/arch_enums.png" alt="Full Arch" width="800"/>
</p>

## Core Data Objects

### USER
*Application Users*

**Attributes:**
| Name | Type | Description |
|------|------|-------------|
| id | uuid (PK) | Unique identifier for the user |
| username | string | User's display name |
| password | string | Authentication credential (should be hashed) |
| currency | int | In-game currency balance for purchasing packs and cards |
| owned_cards | reference (list) | Collection reference to all cards owned by the user |
| owned_packs | reference (list) | Collection reference to all unopened packs |
| open_trades | reference (list)  | Collection reference to active trade listings |
| trade_history | reference (list) | Collection reference to completed trades |

**Relationships:**
- One user can own many cards (1:N with CARD)
- One user can own many packs (1:N with PACK)
- One user can create many open trades (1:N with OPEN_TRADE)
- One user can be the seller in many completed trades (1:N with COMPLETED_TRADE)
- One user can be the buyer in many completed trades (1:N with COMPLETED_TRADE)
- One user can receive many rewards (1:N with REWARDS)

---

### VEHICLE
*Meta object used to represent a CARD or PACK's context.*

**Attributes:**
| Name | Type | Description |
|------|------|-------------|
| id | uuid (PK) | Unique identifier for the vehicle type |
| year | string | Manufacturing year |
| make | string | Vehicle manufacturer (e.g., Nissan, Toyota) |
| model | string | Specific model name |
| stat1 | int | Performance statistic (e.g., horsepower) |
| stat2 | int | Performance statistic (e.g., speed) |
| statN | int | Additional performance statistics (e.g., handling) |
| value | int | Base value of the vehicle |
| image | image | Visual representation of the vehicle |

**Relationships:**
- One vehicle type can have many card instances (1:N with CARD)
- Many vehicles belong to many collections (N:M through COLLECTION relationship)

**Design Note:** The VEHICLE entity is the master data. When a user opens a pack and gets a card, the system creates a new CARD entity that references a VEHICLE and assigns it a grade.

---

### CARD
*An individual Card instance owned by a single USER. Each card is a specific instance of a VEHICLE with an assigned grade/rarity.*

**Attributes:**
| Name | Type | Description |
|------|------|-------------|
| id | uuid (PK) | Unique identifier for this specific card instance |
| user_id | uuid (FK) | References the current owner |
| vehicle_id | uuid (FK) | References the vehicle this card represents |
| collection_id | uuid (FK) | References which collection this card came from |
| grade | GRADE_ENUM | Rarity/quality tier of this specific card |
| value | int | Current market value (may differ from base vehicle value based on grade) |

**Relationships:**
- Each card belongs to exactly one user at a time (N:1 with USER)
- Each card represents exactly one vehicle type (N:1 with VEHICLE)
- Each card originated from one collection (N:1 with COLLECTION)
- Cards can be listed in open trades (1:N with OPEN_TRADE)
- Cards are involved in completed trades (1:N with COMPLETED_TRADE)

**Grade System:**
The GRADE_ENUM defines three rarity tiers:
- `FACTORY`: Common/basic tier
- `LIMITED_RUN`: Rare tier
- `NISMO`: Legendary tier (highest rarity)

---

### PACK
*An unopened pack that contains random cards when opened.*

**Attributes:**
| Name | Type | Description |
|------|------|-------------|
| id | uuid (PK) | Unique identifier for this pack instance |
| user_id | uuid (FK) | References the current owner |
| collection_id | uuid (FK) | Determines which vehicles can be pulled from this pack |
| value | int | The purchase/resale value of the pack |

**Relationships:**
- Each pack belongs to exactly one user (N:1 with USER)
- Each pack is associated with one collection (N:1 with COLLECTION)
- Packs can be awarded through rewards (1:N with REWARDS)

**Lifecycle:**
1. User purchases pack using currency (deducts from USER.currency)
2. Pack is created and linked to user and a specific collection
3. When user opens pack, it's deleted and random cards are generated based on the collection's vehicle pool
4. Generated cards are assigned to the user with randomly determined grades

---

### COLLECTION

Defines a themed set of vehicles and serves as the pool for pack openings.

**Attributes:**
| Name | Type | Description |
|------|------|-------------|
| id | uuid (PK) | Unique identifier for the collection |
| vehicles | reference | Reference to all vehicles included in this collection |
| name | string | Display name (e.g., "JDM Legends", "American Muscle") |
| image | image | Collection artwork/banner |
| pack_price | int | Cost in currency to purchase a pack from this collection |

**Relationships:**
- One collection contains many vehicles (1:N with VEHICLE)
- One collection determines the vehicle pool for many packs (1:N with PACK)
- One collection is the source for many cards (1:N with CARD)

**Purpose:** Collections organize vehicles into themed groups and control pack contents. When a user buys a pack from a specific collection, the cards they receive will only contain vehicles from that collection's pool.

---

## Trading System

### OPEN_TRADE

Represents active trade listings created by users.

**Attributes:**
| Name | Type | Description |
|------|------|-------------|
| id | uuid (PK) | Unique identifier for the trade listing |
| type | TRADE_ENUM | Either FOR_CARD or FOR_PRICE |
| user_id | uuid (FK) | References the user creating the trade (seller) |
| card_id | uuid (FK) | The card being offered by the seller |
| price | int | Currency price (for FOR_PRICE trades) |
| want_card_id | uuid (FK, optional) | Specific card wanted (for FOR_CARD trades) |

**Relationships:**
- Each trade is created by one user (N:1 with USER)
- Each trade offers one card (N:1 with CARD)
- Each trade may want a specific card (N:1 with CARD, optional)

**Trade Types:**

1. **FOR_PRICE**: Simple marketplace listing
   - Seller lists their card with a price
   - Any user with enough currency can buy it
   - `want_card_id` is null
   - Example: "Selling 1995 Skyline GT-R for 5000 currency"

2. **FOR_CARD**: Card-for-card exchange
   - Seller offers their card and specifies a specific card they want
   - Only the owner of the wanted card can complete the trade
   - `price` may still be used for currency adjustments if needed
   - Example: "Trading my Supra for your NSX"

---

### COMPLETED_TRADE

Historical record of all executed trades.

**Attributes:**
| Name | Type | Description |
|------|------|-------------|
| id | uuid (PK) | Unique identifier for the trade record |
| type | TRADE_ENUM | Either FOR_CARD or FOR_PRICE |
| seller_user_id | uuid (FK) | User who offered the card |
| seller_card_id | uuid (FK) | Card that was sold/traded |
| buyer_user_id | uuid (FK) | User who accepted the trade |
| buyer_card_id | uuid (FK, optional) | Card given in exchange (FOR_CARD trades only) |
| executed_date | date | When the trade was completed |
| price | int | Currency exchanged |

**Relationships:**
- Each completed trade has one seller (N:1 with USER)
- Each completed trade has one buyer (N:1 with USER)
- Each completed trade involves at least one card from seller (N:1 with CARD)
- Each completed trade may involve a card from buyer (N:1 with CARD, optional)

**Trade Execution Flow:**

1. **FOR_PRICE Trade:**
   - Buyer must have `price` amount in currency
   - Transfer `price` currency from buyer to seller
   - Transfer card ownership from seller to buyer
   - Delete OPEN_TRADE record
   - Create COMPLETED_TRADE record
   - Create REWARDS entries for both users

2. **FOR_CARD Trade:**
   - Verify buyer owns the wanted card
   - Swap card ownership
   - Handle any currency adjustment via `price`
   - Delete OPEN_TRADE record
   - Create COMPLETED_TRADE record
   - Create REWARDS entries for both users

---

## Rewards System

### REWARDS

Tracks rewards earned by users through various activities (trades, achievements, etc.).

**Attributes:**
| Name | Type | Description |
|------|------|-------------|
| id | uuid (PK) | Unique identifier for the reward |
| user_id | uuid (FK) | User receiving the reward |
| type | REWARD_ENUM | Type of reward |
| item_id | uuid (FK, optional) | References PACK or CARD for item rewards |
| amount | int (optional) | Quantity for currency rewards |
| created_at | timestamp | When reward was earned |
| claimed_at | timestamp (nullable) | When user claimed the reward (null if unclaimed) |

**Relationships:**
- Each reward belongs to one user (N:1 with USER)
- Each reward may reference a pack (N:1 with PACK, optional)
- Each reward may reference a card (N:1 with CARD, optional)

**Reward Types:**

1. **PACK**: User receives an unopened pack
   - `item_id` references PACK.id
   - User must claim to add pack to inventory

2. **CURRENCY**: Non-trade currency reward (achievements, daily bonuses, etc.)
   - `amount` specifies currency quantity
   - No `item_id` needed

3. **CARD_FROM_TRADE**: Card received from a completed trade
   - `item_id` references CARD.id
   - Tracks card changes from trades

4. **CURRENCY_FROM_TRADE**: Currency received from a completed trade
   - `amount` specifies currency quantity
   - Tracks currency earned from selling cards

**Purpose:** The rewards system provides a unified inbox/notification system for all items users receive. This allows for deferred claiming, tracking of income sources, and potential future features like gifting or reward expiration.

---

## Key Workflows

### Purchasing a Pack

1. User selects a collection and initiates purchase
2. System verifies user has sufficient currency (USER.currency >= COLLECTION.pack_price)
3. Deduct pack_price from USER.currency
4. Create new PACK entity with user_id and collection_id
5. Pack appears in user's inventory

### Opening a Pack

1. User selects a pack from their inventory
2. System retrieves the pack's collection_id
3. Query all VEHICLE entities in that collection
4. Generate random cards (typically 3-5):
   - Randomly select vehicles from collection pool
   - Randomly assign grades based on rarity weights
   - Create CARD entities with user_id, vehicle_id, collection_id, and grade
5. Delete the PACK entity
6. Display new cards to user

### Creating a Trade (FOR_PRICE)

1. User selects a card from their collection
2. User sets a price
3. Create OPEN_TRADE with type=FOR_PRICE, card_id, price
4. Trade appears in marketplace for other users

### Completing a Trade (FOR_PRICE)

1. Buyer views open trade and clicks buy
2. Verify buyer has sufficient currency
3. Transfer currency: buyer.currency -= price, seller.currency += price
4. Transfer card: card.user_id = buyer_id
5. Create COMPLETED_TRADE record
6. Create REWARDS entry (type=CARD_FROM_TRADE) for buyer
7. Create REWARDS entry (type=CURRENCY_FROM_TRADE) for seller
8. Delete OPEN_TRADE

### Creating a Trade (FOR_CARD)

1. User selects a card they own
2. User specifies a specific card they want (by searching or browsing)
3. Create OPEN_TRADE with type=FOR_CARD, card_id, want_card_id
4. Only the owner of want_card_id can see and accept this trade

### Completing a Trade (FOR_CARD)

1. Owner of wanted card accepts trade
2. Swap card ownership:
   - seller_card.user_id = buyer_id
   - buyer_card.user_id = seller_id
3. Create COMPLETED_TRADE record
4. Create REWARDS entries for both users (type=CARD_FROM_TRADE)
5. Delete OPEN_TRADE

---

## Design Considerations

### Scalability
- All primary keys use UUIDs for distributed system compatibility
- Completed trades are archived separately from open trades for query performance
- Indexes should be added on foreign keys (user_id, vehicle_id, collection_id)

### Data Integrity
- Card ownership is atomic (each card has exactly one owner)
- Trades are transactional (all changes succeed or all fail)
- Currency balance must never go negative
- Packs are deleted when opened (prevents duplicate card generation)

### Future Enhancements
- Add `inventory_count` to VEHICLE for limited availability
- Add `trade_locked` boolean to CARD for preventing certain cards from being traded
- Add `expires_at` to REWARDS for time-limited rewards
- Add `daily_reward_claimed_at` to USER for daily login bonuses
- Consider adding AUCTION table for timed bidding system

### Security Notes
- USER.password should be hashed (bcrypt, argon2)
- Trade completion should use database transactions
- Validate all currency operations to prevent exploitation
- Implement rate limiting on pack purchases and trade creation

---

## Summary

This architecture supports a full-featured trading card game with:
- ✅ Pack purchasing and opening mechanics
- ✅ Card ownership and grading system
- ✅ Multiple trade types (currency and card-for-card)
- ✅ Complete trade history tracking
- ✅ Unified rewards/inbox system
- ✅ Scalable collection-based content organization

The schema is normalized and uses clear relationships to maintain data integrity while supporting complex trading mechanics.